using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authetication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authetication.Controllers
{
    public class LoginController : Controller
    {
        UserDataAccessLayer objUser = new UserDataAccessLayer();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterUser()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult RegisterUser([Bind] UserDetails user)
        {
            if (ModelState.IsValid)
            {
                string RegistrationStatus = objUser.RegisterUser(user);
                if (RegistrationStatus == "Success")
                {
                    ModelState.Clear();
                    TempData["Success"] = "Registration Successful!";
                    return View();
                }
                else
                {
                    TempData["Fail"] = "This User ID already exists. Registration Failed.";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserLogin()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin([Bind] UserDetails user)
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");

            if (ModelState.IsValid)
            {
                string LoginStatus = objUser.ValidateLogin(user);

                if (LoginStatus == "Success")
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserID),
                        new Claim("BoardingPassNumber",123456.ToString()),

                        new Claim("RequirementOne","Ok"),
                        new Claim("IsEmployee","Yes")
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("UserHome", "User");
                }
                else
                {
                    TempData["UserLoginFailed"] = "Login Failed.Please enter correct credentials";
                    return View();
                }
            }
            else
                return View();

        }
    }
}