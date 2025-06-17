using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{

    public class AdminController : Controller
    {
        // Login Page
        public IActionResult Login()
        {
            return View();
        }

        // Login Action
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Implement your authentication logic here
            if (username == "admin" && password == "admin") // Simple check for demo purposes
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user
                await HttpContext.SignInAsync(principal);

                // Redirect to the Admin Dashboard
                return RedirectToAction("Index","Gnfc");
            }

            // If invalid login, return to login page with an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

      
        public IActionResult List()
        {
            return View();
        }

        // Logout Action
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login","Admin");
        }
    }
}
