using FluentResults;
using HRS.Domain;
using HRS.Models;
using HRS.Services;
using HRS.ViewModels;
using HRS.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using A = Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticationService _authService;
        private readonly IAuthenticationSessionProvider _authenticationServiceProvider;
        const string APIKEY = "5555"; //This is suppose to be the APIKEy for Tracker authenticant

        public HomeController(ILogger<HomeController> logger, IAuthenticationService authService, IAuthenticationSessionProvider authenticationServiceProvider)
        {
            _logger = logger;
            _authService = authService;
            _authenticationServiceProvider = authenticationServiceProvider;
        }
        [HttpGet("", Name = "Home")]
        [AllowAnonymous]
        public IActionResult Index() => View();

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Index([FromForm] LoginRequest credentials)
        {
            Result<(string authToken, Domain.UserModel user)> res = await _authService.Login(credentials);

            if (res.IsSuccess)
            {
                 _authenticationServiceProvider.SetAuthenticatedSession(res.Value.user.Id, res.Value.authToken);
                return RedirectToRoute("Dashboard", res.Value.user);

            }
            ViewData["error"] = "Login failed";
            return View();

        }









        public IActionResult Signup()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromForm] SignupRequest credentials)
        {
            var res = await _authService.Register(credentials);

            if (res.IsFailed)
                ViewData["error"] = "Signup Fail";

            ViewData["error"] = "Signup Success";
            return View();

        }






        public IActionResult TrackerLogin()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost("TrackerLogin")]
        public async Task<IActionResult> TrackerLogin([FromForm] LoginRequest credentials)
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
                //Initialise A = Using Newtonsoft to resolve conflict between Newtonsoft and text.Json
                var results = A.JsonConvert.DeserializeObject<LoginResponse>(response);

                Result<(string authToken, Domain.UserDTO user)> data = await _authService.LoginUsing3rdParty(credentials.Email, results.Key);

                _authenticationServiceProvider.SetAuthenticatedSession(data.Value.user.Id, data.Value.authToken);


                return RedirectToRoute("Dashboard", new IndexViewModel { Email = data.Value.user.Email, Name = data.Value.user.Name });
            }

            ViewData["error"] = "Login failed";
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

class LoginResponse
{
    public string Email { get; set; }
    public string Key { get; set; }
}
