using ESTIGamingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ESTIGamingWebsite.Controllers
{
    public class GenreController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private string apiPath = "https://localhost:7163/api/";

        public GenreController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);


            // Pedir Generos
            var requestGenres = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Genre");

            var response = await client.SendAsync(requestGenres);

            var list = new List<Genre>();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var genres = await JsonSerializer.DeserializeAsync
                    <List<Genre>>(responseStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                ViewBag.Genre = genres;
                list = genres;
            }
            else
            {
                ViewBag.Genres = new List<Genre>();
            }

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Genre genre)
        {
            var requestPostGenre = new HttpRequestMessage(HttpMethod.Post,
                apiPath + "Genre/");

            var g = genre;

            requestPostGenre.Content = new StringContent(
                JsonSerializer.Serialize(genre,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var response = await client.PostAsJsonAsync(
                apiPath + "Genre/", genre);
            response.EnsureSuccessStatusCode();

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return Redirect("/Genre/Index");
        }

        [Route("/Genre/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestGenre = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Genre/" + id);

            var genreEdit = new Genre();

            var response = await client.SendAsync(requestGenre);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var genre = await JsonSerializer.DeserializeAsync
                    <Genre>(responseStream, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });
                genreEdit = genre;
            }
            else
            {
                return Redirect("/Genre/Index");
            }

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return View(genreEdit);
        }

        [HttpPost]
        [Route("/Genre/{id:int}/Edit")]
        public async Task<IActionResult> Edit(Genre edit, int id)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestGenre = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Genre/" + id);

            var response = await client.SendAsync(requestGenre);

            Genre genre = new Genre();

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                genre = await JsonSerializer.DeserializeAsync<Genre>
                    (stream, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });
            }

            var requestPutGenre = new HttpRequestMessage(HttpMethod.Put,
                apiPath + "Genre/" + id);

            edit.Id = id;

            requestPutGenre.Content = new StringContent(
                JsonSerializer.Serialize(edit, new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true }), Encoding.UTF8, "application/json");

            await client.SendAsync(requestPutGenre);

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return Redirect("/Genre/Index");
        }

        [HttpGet]
        [Route("/Genre/{id:int}/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var requestDeleteGenre = new HttpRequestMessage(HttpMethod.Delete,
                                            apiPath + "Genre/" + id);

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);
            var response = await client.SendAsync(requestDeleteGenre);

            response.EnsureSuccessStatusCode();

            return Redirect("/Genre/Index");
        }
    }
}
