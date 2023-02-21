using Microsoft.AspNetCore.Mvc;
using ESTIGamingWebsite.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web.Helpers;

namespace ESTIGamingWebsite.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string apiPath = "https://localhost:7163/api/";

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var error = HttpContext.Session.GetString("loginError");
            if (error != null)
            {
                HttpContext.Session.Clear();
                ViewBag.Error = true;
            }
            return View();
        }

        public IActionResult Register()
        {
            var emailError = HttpContext.Session.GetString("registerEmailError");
            var passwordError = HttpContext.Session.GetString("registerPasswordError");
            if (emailError != null)
            {
                HttpContext.Session.Clear();
                ViewBag.EmailError = true;
            }
            if (passwordError != null)
            {
                HttpContext.Session.Clear();
                ViewBag.PasswordError = true;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckRegister(IFormCollection values)
        {
            var name = values["name"].ToString();
            var email = values["email"].ToString();
            var password = values["password"].ToString();
            var confirmPassword = values["confirmPassword"].ToString();

            if (password.Equals(confirmPassword))
            {
                var user = new User()
                {
                    Name = name,
                    Email = email,
                    Password = Crypto.HashPassword(password),
                    RoleId = 2
                };

                var requestPostUser = new HttpRequestMessage(HttpMethod.Post,
                    apiPath + "User");
                

                requestPostUser.Content = new StringContent(
                    JsonSerializer.Serialize(user,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Add(new 
                    MediaTypeWithQualityHeaderValue("application/json"));


                var response = await client.PostAsJsonAsync(
                    apiPath + "User/", user);

                response.EnsureSuccessStatusCode();

                return Redirect("/Login/Index");
            }

            return Redirect("/Login/Register");
        }

        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection values)
        {
            var email = values["email"].ToString();
            var password = values["password"].ToString();

            var requestUsers = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "User");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(requestUsers);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var users = await JsonSerializer.DeserializeAsync<List<User>>
                    (responseStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                var user = new User();

                foreach (var uniqueUser in users)
                {
                    if (uniqueUser.Email == email)
                    {
                        user.Email = uniqueUser.Email;
                        user.Password = uniqueUser.Password;
                        user.Id = uniqueUser.Id;
                        user.RoleId = uniqueUser.RoleId;
                    }
                }

                if (user != null && (user.Password == password || Crypto.VerifyHashedPassword(user.Password, password)))
                {
                    var requestToken = new HttpRequestMessage(HttpMethod.Get,
                        apiPath + "Token");

                    var client2 = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var response2 = await client2.SendAsync(requestToken);
                    if (response2.IsSuccessStatusCode)
                    {
                        using var responseStream2 = await response2.Content.ReadAsStreamAsync();
                        var token = await JsonSerializer.DeserializeAsync
                            <Token>(responseStream2, new JsonSerializerOptions
                            { PropertyNameCaseInsensitive = true });
                        HttpContext.Session.SetString("token", token.Value.ToString());
                        HttpContext.Session.SetString("tokenExpiration", token.ExpirationDate.ToString());
                    }
                    else
                    {
                        return Redirect("/");
                    }

                    ViewBag.UserType = user.RoleId.ToString();

                    HttpContext.Session.SetString("userType", user.RoleId.ToString());
                    return Redirect("/Game/Index");
                }
            }

            HttpContext.Session.SetString("loginError", "Login Error");
            return Redirect("/Login/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewBag.UserType = null;
            return Redirect("/");
        }
    }
}
