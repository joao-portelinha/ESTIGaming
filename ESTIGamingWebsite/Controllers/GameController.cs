using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using ESTIGamingWebsite.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESTIGamingWebsite.Controllers
{
    public class GameController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private string apiPath = "https://localhost:7163/api/";

        public GameController(IHttpClientFactory clientFactory)
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

            var responseGenres = await client.SendAsync(requestGenres);

            if (responseGenres.IsSuccessStatusCode)
            {
                using var responseStream = await responseGenres.Content.ReadAsStreamAsync();
                var allGenres = await JsonSerializer.DeserializeAsync<List<Genre>>
                    (responseStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                ViewBag.Genres = allGenres;
            }

            // Pedir Plataformas
            var requestPlatforms = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Platform");

            var responsePlatforms = await client.SendAsync(requestPlatforms);

            if (responsePlatforms.IsSuccessStatusCode)
            {
                using var responseStream = await responsePlatforms.Content.ReadAsStreamAsync();
                var allPlatforms = await JsonSerializer.DeserializeAsync<List<Platform>>
                    (responseStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                ViewBag.Platforms = allPlatforms;
            }

            // Pedir Jogos
            var requestGames = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Game");

            var response = await client.SendAsync(requestGames);

            var list = new List<Game>();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var games = await JsonSerializer.DeserializeAsync
                    <List<Game>>(responseStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                ViewBag.Games = games;
                list = games;
            }
            else
            {
                ViewBag.Games = new List<Game>();
            }

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return View(list);
        }

        [HttpGet]
        [Route("/Game/Details/{gameId:int}")]
        public async Task<IActionResult> Details(int gameId)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestGame = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Game/" + gameId);

            var response = await client.SendAsync(requestGame);

            var game = new Game();

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                game = await JsonSerializer.DeserializeAsync<Game>
                    (stream, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });
            }
            else
            {
                return Redirect("/Game/Index");
            }

            var requestGenre = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Genre/" + game.GenreId);

            var responseGenre = await client.SendAsync(requestGenre);

            var genre = "";

            if (responseGenre.IsSuccessStatusCode)
            {
                using var stream2 = await responseGenre.Content.ReadAsStreamAsync();
                var gameGenre = await JsonSerializer.DeserializeAsync<Genre>
                    (stream2, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });

                genre = gameGenre.Name;
                ViewBag.Genre = genre;
            }

            ViewBag.UserType = HttpContext.Session.GetString("userType");
            ViewBag.GameId = game.Id;
            HttpContext.Session.SetString("Jogo", game.Id.ToString());

            return View(game);
        }

        public async Task<IActionResult> Create()
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestGenres = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Genre");

            var response = await client.SendAsync(requestGenres);

            var genres = new List<Genre>();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var AllGenres = await JsonSerializer.DeserializeAsync<List<Genre>>
                    (responseStream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                genres = AllGenres;
            }

            var requestPlatforms = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Platform");

            var response2 = await client.SendAsync(requestPlatforms);

            var platforms = new List<Platform>();

            if (response2.IsSuccessStatusCode)
            {
                using var responseStream2 = await response2.Content.ReadAsStreamAsync();
                var allPlatforms = await JsonSerializer.DeserializeAsync<List<Platform>>
                    (responseStream2, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                platforms = allPlatforms;
            }

            ViewData["GenreId"] = new SelectList(genres, "Id", "Name");
            ViewData["PlatformId"] = new SelectList(platforms, "Id", "Name");
            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Game game)
        {
            var requestPostGame = new HttpRequestMessage(HttpMethod.Post,
                apiPath + "Game/");

            var g = game;

            requestPostGame.Content = new StringContent(
                JsonSerializer.Serialize(game,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var response = await client.PostAsJsonAsync(
                apiPath + "Game/", game);
            response.EnsureSuccessStatusCode();

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return Redirect("/Game/Index");
        }

        [Route("/Game/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestGame = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Game/" + id);

            var gameEdit = new Game();

            var response = await client.SendAsync(requestGame);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var game = await JsonSerializer.DeserializeAsync
                    <Game>(responseStream, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });
                gameEdit = game;

                var requestGenre = new HttpRequestMessage(HttpMethod.Get,
                    apiPath + "Genre");

                var responseGenres = await client.SendAsync(requestGenre);

                var genres = new List<Genre>();

                if (responseGenres.IsSuccessStatusCode)
                {
                    using var stream = await responseGenres.Content.ReadAsStreamAsync();
                    var allGenres = await JsonSerializer.DeserializeAsync
                        <List<Genre>>
                        (stream, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    genres = allGenres;
                }

                var requestPlatform = new HttpRequestMessage(HttpMethod.Get,
                    apiPath + "Platform");

                var responsePlatform = await client.SendAsync(requestPlatform);

                var platforms = new List<Platform>();

                if (responsePlatform.IsSuccessStatusCode)
                {
                    using var stream = await responsePlatform.Content.ReadAsStreamAsync();
                    var allPlatforms = await JsonSerializer.DeserializeAsync
                        <List<Platform>>
                        (stream, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    platforms = allPlatforms;
                }
                ViewData["GenreId"] = new SelectList(genres, "Id", "Name");
                ViewData["PlatformId"] = new SelectList(platforms, "Id", "Name");
            }
            else
            {
                return Redirect("/Game/Index");
            }

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return View(gameEdit);
        }

        [HttpPost]
        [Route("/Game/{id:int}/Edit")]
        public async Task<IActionResult> Edit(Game edit, int id)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

            var requestGame = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Game/" + id);

            var response = await client.SendAsync(requestGame);

            Game game = new Game();

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                game = await JsonSerializer.DeserializeAsync<Game>
                    (stream, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });
            }

            var requestPutGame = new HttpRequestMessage(HttpMethod.Put,
                apiPath + "Game/" + id);

            edit.Id = id;

            requestPutGame.Content = new StringContent(
                JsonSerializer.Serialize(edit, new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true }), Encoding.UTF8, "application/json");

            await client.SendAsync(requestPutGame);

            ViewBag.UserType = HttpContext.Session.GetString("userType");

            return Redirect("/Game/Index");
        }
    }
}
