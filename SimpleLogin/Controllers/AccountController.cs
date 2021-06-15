using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SimpleLogin.Models;

namespace SimpleLogin.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("index", "home");

                ModelState.AddModelError(string.Empty, "You're already Logged In");
                return View("login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "flavio.hfs@gmail.com")
                {
                    var claims = new List<Claim>();

                    try
                    {
                        claims.Add(new Claim(ClaimTypes.Name, model.Email));
                        claims.Add(new Claim(ClaimTypes.Role, "admin"));
                        var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                        var authenticationManager = Request.HttpContext;

                        await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = true });

                        return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction("index", "home");
                    }
                    catch (Exception ex)
                    {
                        // Info  
                        throw;
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View("login", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var authenticationManager = Request.HttpContext;
                await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return RedirectToPage("/");
            return RedirectToAction("index", "home");
        }
    }
}
