using ESTIGamingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ESTIGamingWebsite.Controllers
{
    public class PlatformController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private string apiPath = "https://localhost:7163/api/";

        public PlatformController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);


            // Pedir Plataformas
            var requestPlatforms = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Platform");

            var response = await client.SendAsync(requestPlatforms);

            var list = new List<Platform>();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var platforms = await JsonSerializer.DeserializeAsync
                    <List<Platform>>(responseStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                ViewBag.Platform = platforms;
                list = platforms;
            }
            else
            {
                ViewBag.Platform = new List<Platform>();
            }

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Platform platform)
        {
            var requestPostPlatform = new HttpRequestMessage(HttpMethod.Post,
                apiPath + "Platform/");

            var p = platform;

            requestPostPlatform.Content = new StringContent(
                JsonSerializer.Serialize(platform,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var response = await client.PostAsJsonAsync(
                apiPath + "Platform/", platform);
            response.EnsureSuccessStatusCode();

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return Redirect("/Platform/Index");
        }

        [Route("/Platform/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestPlatform = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Platform/" + id);

            var platformEdit = new Platform();

            var response = await client.SendAsync(requestPlatform);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var platform = await JsonSerializer.DeserializeAsync
                    <Platform>(responseStream, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });
                platformEdit = platform;
            }
            else
            {
                return Redirect("/Platform/Index");
            }

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return View(platformEdit);
        }

        [HttpPost]
        [Route("/Platform/{id:int}/Edit")]
        public async Task<IActionResult> Edit(Platform edit, int id)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestPlatform = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Platform/" + id);

            var response = await client.SendAsync(requestPlatform);

            Platform platform = new Platform();

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                platform = await JsonSerializer.DeserializeAsync<Platform>
                    (stream, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });
            }

            var requestPutPlatform = new HttpRequestMessage(HttpMethod.Put,
                apiPath + "Platform/" + id);

            edit.Id = id;

            requestPutPlatform.Content = new StringContent(
                JsonSerializer.Serialize(edit, new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true }), Encoding.UTF8, "application/json");

            await client.SendAsync(requestPutPlatform);

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return Redirect("/Platform/Index");
        }

        [HttpGet]
        [Route("/Platform/{id:int}/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var requestDeletePlatform = new HttpRequestMessage(HttpMethod.Delete,
                                            apiPath + "Platform/" + id);

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);
            var response = await client.SendAsync(requestDeletePlatform);

            response.EnsureSuccessStatusCode();

            return Redirect("/Platform/Index");
        }
    }
}
