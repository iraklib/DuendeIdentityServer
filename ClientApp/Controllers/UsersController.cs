using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClientApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public UsersController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

       
        [HttpPost]
        public async Task<IActionResult> CallUsers()
        {
            // discover endpoints from metadata
            var client = _clientFactory.CreateClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44301");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.WriteLine(disco.Exception);
                return RedirectToAction("Index");
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "MPC",
                ClientSecret = "secret",
                Scope = "apigee"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.WriteLine(tokenResponse.ErrorDescription);
                return RedirectToAction("Index");
            }

            Console.WriteLine(tokenResponse.AccessToken);

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken!); // AccessToken is always non-null when IsError is false

            var response = await apiClient.GetAsync("https://localhost:44302/api/users/100");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                return RedirectToAction("Index");
            }

            var doc = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
            TempData["ApiResponse"] = doc;

            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            if (TempData["ApiResponse"] != null)
            {
                ViewBag.ApiResponse = TempData["ApiResponse"];
            }
            return View();
        }
    }
}
