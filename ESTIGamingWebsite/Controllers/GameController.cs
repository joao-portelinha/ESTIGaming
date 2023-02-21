using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using ESTIGamingWebsite.Models;

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
            var requestGame = new HttpRequestMessage(HttpMethod.Get,
                apiPath + "Game/" + gameId);

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Token", token);

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

            ViewBag.UserType = HttpContext.Session.GetString("userType");
            ViewBag.GameId = game.Id;
            HttpContext.Session.SetString("alojamento", game.Id.ToString());

            return View(game);
        }

        //public async Task<IActionResult> Create()
        //{
        //    var requestTipoAlojamentos = new HttpRequestMessage(HttpMethod.Get,
        //        apiPath + "TipoAlojamentos");

        //    var client = _clientFactory.CreateClient();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var token = HttpContext.Session.GetString("token");
        //    client.DefaultRequestHeaders.Add("Token", token);

        //    var response = await client.SendAsync(requestTipoAlojamentos);

        //    var tipos = new List<TipoAlojamento>();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        using var responseStream = await response.Content.ReadAsStreamAsync();
        //        var tipoAlojamentos = await JsonSerializer.DeserializeAsync<List<TipoAlojamento>>
        //            (responseStream, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });
        //        tipos = tipoAlojamentos;
        //    }

        //    ViewData["TipoAlojamentoId"] = new SelectList(tipos, "TipoAlojamentoId", "Tipo");
        //    ViewBag.UserType = HttpContext.Session.GetString("userType");

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Store(Alojamento alojamento)
        //{
        //    var requestPostAlojamento = new HttpRequestMessage(HttpMethod.Post,
        //        apiPath + "Alojamentos/");

        //    requestPostAlojamento.Content = new StringContent(
        //        JsonSerializer.Serialize(alojamento,
        //        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
        //        Encoding.UTF8, "application/json");

        //    var client = _clientFactory.CreateClient();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var token = HttpContext.Session.GetString("token");
        //    client.DefaultRequestHeaders.Add("Token", token);

        //    var response = await client.PostAsJsonAsync(
        //        apiPath + "Alojamentos/", alojamento);
        //    response.EnsureSuccessStatusCode();

        //    ViewBag.UserType = HttpContext.Session.GetString("userType");

        //    return Redirect("/Alojamentos/Index");
        //}

        //[Route("/Alojamentos/{id:int}/Edit")]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var requestAlojamento = new HttpRequestMessage(HttpMethod.Get,
        //        apiPath + "Alojamentos/" + id);

        //    var client = _clientFactory.CreateClient();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var token = HttpContext.Session.GetString("token");
        //    client.DefaultRequestHeaders.Add("Token", token);

        //    var alojamentoEditar = new Alojamento();

        //    var response = await client.SendAsync(requestAlojamento);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        using var responseStream = await response.Content.ReadAsStreamAsync();
        //        var alojamento = await JsonSerializer.DeserializeAsync
        //            <Alojamento>(responseStream, new JsonSerializerOptions
        //            { PropertyNameCaseInsensitive = true });
        //        alojamentoEditar = alojamento;

        //        var requestTipoAlojamentos = new HttpRequestMessage(HttpMethod.Get,
        //            apiPath + "TipoAlojamentos");

        //        var responseTipo = await client.SendAsync(requestTipoAlojamentos);

        //        var tipos = new List<TipoAlojamento>();

        //        if (responseTipo.IsSuccessStatusCode)
        //        {
        //            using var stream = await responseTipo.Content.ReadAsStreamAsync();
        //            var tipoAlojamentos = await JsonSerializer.DeserializeAsync
        //                <List<TipoAlojamento>>
        //                (stream, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //            tipos = tipoAlojamentos;
        //        }

        //        ViewData["TipoAlojamentoId"] = new SelectList(tipos, "TipoAlojamentoId", "Tipo");
        //    }
        //    else
        //    {
        //        return Redirect("/Alojamentos/Index");
        //    }

        //    ViewBag.UserType = HttpContext.Session.GetString("userType");

        //    return View(alojamentoEditar);
        //}

        //[HttpPost]
        //[Route("/Alojamentos/{id:int}/Edit")]
        //public async Task<IActionResult> Edit(Alojamento novo, int id)
        //{
        //    var requestAlojamento = new HttpRequestMessage(HttpMethod.Get,
        //        apiPath + "Alojamentos/" + id);

        //    var client = _clientFactory.CreateClient();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var token = HttpContext.Session.GetString("token");
        //    client.DefaultRequestHeaders.Add("Token", token);

        //    var response = await client.SendAsync(requestAlojamento);

        //    Alojamento aloj = new Alojamento();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        using var stream = await response.Content.ReadAsStreamAsync();
        //        aloj = await JsonSerializer.DeserializeAsync<Alojamento>
        //            (stream, new JsonSerializerOptions
        //            { PropertyNameCaseInsensitive = true });
        //    }

        //    var requestPutAlojamento = new HttpRequestMessage(HttpMethod.Put,
        //        apiPath + "Alojamentos/" + id);

        //    novo.AlojamentoId = id;

        //    requestPutAlojamento.Content = new StringContent(
        //        JsonSerializer.Serialize(novo, new JsonSerializerOptions
        //        { PropertyNameCaseInsensitive = true }), Encoding.UTF8, "application/json");

        //    await client.SendAsync(requestPutAlojamento);

        //    ViewBag.UserType = HttpContext.Session.GetString("userType");

        //    return Redirect("/Alojamentos/Index");
        //}
    }
}
