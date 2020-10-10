using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Client.Models;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        readonly HttpClient client = new HttpClient();
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

            client.BaseAddress = new Uri("https://localhost:44324/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        public IActionResult Index()
        {
            var Id = HttpContext.Session.GetString("Username");
            if (Id != null)
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var myContent = JsonConvert.SerializeObject(model);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result2 = client.PostAsync("Account/login", byteContent).Result;

                    if (result2.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                    {
                        var tes = await result2.Content.ReadAsAsync<Token>();
                        var token = "Bearer " + tes.AccessToken;
                        var exptoken = tes.ExpireToken;
                        var reftoken = tes.RefreshToken;
                        var expreftoken = tes.ExpireRefreshToken;
                        HttpContext.Session.SetString("Username", model.Email);
                        HttpContext.Session.SetString("JWToken", token);
                        HttpContext.Session.SetString("ExpToken", exptoken.ToString());
                        HttpContext.Session.SetString("RefreshToken", reftoken);
                        HttpContext.Session.SetString("ExpRefToken", expreftoken.ToString());

                        _logger.LogInformation("User logged in.");
                        return RedirectToAction(nameof(Index));
                    }
                    else if(result2.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {
                        return View(model);
                    }
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Logout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public async Task<IActionResult> Register(RegisterViewModel modelRegis, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {
                    //var newemail = _userManager.FindByEmailAsync(modelRegis.Email);
                    //if(newemail!=)
                    var user = new IdentityUser { };
                    user.Id = modelRegis.Email;
                    user.UserName = modelRegis.Email;
                    user.Email = modelRegis.Email;
                    user.PasswordHash = modelRegis.Password;
                    var result = await _userManager.CreateAsync(user, modelRegis.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Your Email Has Been Registered");
                        return View(modelRegis);
                    }
                }
                ModelState.AddModelError(string.Empty, "Your Email Has Been Registered");
                return View(modelRegis);
            }
            catch(Exception e)
            {
                _logger.LogInformation($"Something Went Wrong{e}");
                return View(modelRegis);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User Logged Out");
            HttpContext.Session.Remove("Username");
            return RedirectToAction(nameof(Index));
        }
    }
}