using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using WebGatos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WebGatos.Controllers
{

    public class GatoFactsController : Controller
    {
        // declaro el cliente
        private readonly HttpClient _httpClient;
        // constructor
        
        public GatoFactsController(IHttpClientFactory httpClientFactory)
        {
            // Inicializo el cliente y le paso la url basica
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://catfact.ninja/");
        }
        public IActionResult Titulo()
        {
            return View();
        }
        // obtengo un dato
        public async Task<IActionResult> Mostrar()
        {
            GatoFactsModelo oGato = new GatoFactsModelo();
            // llamo a la API
            var respuesta = await _httpClient.GetAsync("fact");
            // valido si la respuesta fue existosa
            if (respuesta.IsSuccessStatusCode)
            {
                // obtnego la respuesta
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var dato = JsonConvert.DeserializeObject<GatoFactsModelo>(contenido);
                return View(dato);
            }
            return View();
        }

        public async Task<IActionResult> Paginas()
        {
            TempData["paginaActual"] = 1;
            // llamo a la API
            var respuesta = await _httpClient.GetAsync("facts?limit=5");
            // valido si la respuesta fue existosa
            if (respuesta.IsSuccessStatusCode)
            {
                // obtnego la respuesta
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<PaginasModelo>(contenido);
                return View(datos);
            }
            return View();
        }

        public async Task<IActionResult> MoverPagina(bool delante)
        {
            int actual = Convert.ToInt32(TempData.Peek("paginaActual"));
            if (delante)
            {
                if (actual < 67)
                    actual += 1;
                else
                    actual = 1;
            }
            if (!delante)
            {
                if (actual > 1)
                    actual = actual - 1;
                else
                    actual = 67;
            }
            TempData["paginaActual"] = actual;
            // llamo a la API
            var respuesta = await _httpClient.GetAsync("facts?limit=5&page=" + actual);
            // valido si la respuesta fue existosa
            if (respuesta.IsSuccessStatusCode)
            {
                // obtnego la respuesta
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<PaginasModelo>(contenido);
                return View("Paginas",datos);
            }
            return View("Paginas");
        }
    }
}
