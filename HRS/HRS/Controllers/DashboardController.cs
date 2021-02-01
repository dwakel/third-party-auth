using FluentResults;
using HRS.Services;
using HRS.ViewModels;
using HRS.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using A = Newtonsoft.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly IAuthenticationSessionProvider _authenticationServiceProvider;
        const string APIKEY = "5555"; //This is suppose to be the APIKEy for Tracker authenticant

        public DashboardController(IAuthenticationService authService, IAuthenticationSessionProvider authenticationServiceProvider)
        {
            _authService = authService;
            _authenticationServiceProvider = authenticationServiceProvider;
        }

        [HttpGet("Dashboard", Name = "Dashboard")]
        public IActionResult Index(IndexViewModel viewModel = null)
        {

            return View(new IndexViewModel { 
                Email = viewModel.Email,
                Name = viewModel.Name
            });
        }


        public IActionResult LinkTracker() => View();


        [HttpPost("TrackerRegister")]
        public async Task<IActionResult> LinkTracker([FromForm] LoginRequest credentials)
        {
            HttpClient http = new HttpClient();

            StringContent body = new StringContent(JsonSerializer.Serialize(new
            {
                email = credentials.Email,
                password = credentials.Password
            }), Encoding.UTF8, "application/json");

            HttpResponseMessage res = await http.PostAsync($"https://localhost:5000/Authentication/3rdPartyAuth?ApiKey={APIKEY}", body);

            if (res.IsSuccessStatusCode)
            {
                var response = await res.Content.ReadAsStringAsync();
                var results =  A.JsonConvert.DeserializeObject<LoginResponse>(response);

                Result<Domain.UserDTO> data = await _authService.Register3rdParty(credentials.Email, results.Key);

                if (data is null)
                {
                    ViewData["3rdPartMessage"] = "Account link fail";
                    return View();
                }

                //_authenticationServiceProvider.SetAuthenticatedSession(data.Value.user.Id, data.Value.authToken);

                ViewData["3rdPartMessage"] = "Account link Success";
                return RedirectToRoute("Dashboard", new IndexViewModel() { Key = results.Key, Name = data.Value.Name, Email = data.Value.Email });
            }

            ViewData["3rdPartError"] = "Account link fail";
            return View();

        }
    }
}
