using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCApp.Filters;

namespace MVCApp.Controllers
{
    [LoginRequired]
    public class ReportsController : Controller
    {
        AppointmentService appointmentService;
        DoctorService doctorService;

        public ReportsController(AppointmentService appointmentService, DoctorService doctorService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
        }

        [HttpGet]
        public IActionResult AppointmentReport(DateTime? fromDate, DateTime? toDate)
        {
            var data = appointmentService.GetReport(fromDate, toDate);
            return View(data);
        }

        [HttpGet]
        public IActionResult DoctorSchedule(int? doctorId, DateTime? date)
        {
            ViewBag.DoctorId = new SelectList(doctorService.GetActive(), "DoctorId", "Name", doctorId);
            ViewBag.SelectedDate = (date ?? DateTime.Today).ToString("yyyy-MM-dd");

            if (doctorId == null)
            {
                return View(new List<BLL.DTOs.AppointmentDTO>());
            }

            var data = appointmentService.GetDoctorSchedule(doctorId.Value, date ?? DateTime.Today);
            return View(data);
        }

        [HttpGet]
        public IActionResult OverdueAlerts()
        {
            var data = appointmentService.GetOverdueAlerts();
            return View(data);
        }
    }
}