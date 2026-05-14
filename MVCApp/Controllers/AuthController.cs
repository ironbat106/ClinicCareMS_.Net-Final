using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVCApp.Controllers
{
    public class AuthController : Controller
    {
        AuthService service;

        public AuthController(AuthService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDTO());
        }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = service.Login(dto);
                    var uType = service.GetUserType(user);

                    // Faculty demo style session keys
                    HttpContext.Session.SetString("Uname", user.UserName);
                    HttpContext.Session.SetInt32("UType", uType);

                    // Existing ClinicCareMS session keys, used by layout and filters
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("Role", user.Role);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View(new RegDTO());
        }

        [HttpPost]
        public IActionResult Registration(RegDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Register(dto);
                    TempData["Msg"] = "Registration successful. Please login.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(dto);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
