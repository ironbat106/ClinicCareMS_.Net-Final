using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Filters;
using MVCApp.Models;
using System.Diagnostics;

namespace MVCApp.Controllers
{
    [LoginRequired]
    public class HomeController : Controller
    {
        AppointmentService appointmentService;

        public HomeController(AppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            var data = appointmentService.GetDashboard();
            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
