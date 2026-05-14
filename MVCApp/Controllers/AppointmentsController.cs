using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCApp.Filters;

namespace MVCApp.Controllers
{
    [LoginRequired]
    public class AppointmentsController : Controller
    {
        AppointmentService appointmentService;
        DoctorService doctorService;
        PatientService patientService;

        public AppointmentsController(AppointmentService appointmentService, DoctorService doctorService, PatientService patientService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.patientService = patientService;
        }

        public IActionResult Index()
        {
            return View(appointmentService.Get());
        }

        public IActionResult Details(int id)
        {
            var data = appointmentService.Get(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropDowns();
            return View(new AppointmentDTO());
        }

        [HttpPost]
        public IActionResult Create(AppointmentDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appointmentService.Create(dto);
                    TempData["Msg"] = "Appointment created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            LoadDropDowns(dto.PatientId, dto.DoctorId);
            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = appointmentService.Get(id);
            if (data == null) return NotFound();
            LoadDropDowns(data.PatientId, data.DoctorId);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(AppointmentDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appointmentService.Update(dto);
                    TempData["Msg"] = "Appointment updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            LoadDropDowns(dto.PatientId, dto.DoctorId);
            return View(dto);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = appointmentService.Get(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(int id, string decision)
        {
            if (decision == "Yes")
            {
                appointmentService.Delete(id);
                TempData["Msg"] = "Appointment deleted successfully.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            try
            {
                appointmentService.ChangeStatus(id, status);
                TempData["Msg"] = "Status changed successfully.";
            }
            catch (Exception ex)
            {
                TempData["Err"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        private void LoadDropDowns(int selectedPatient = 0, int selectedDoctor = 0)
        {
            ViewBag.PatientId = new SelectList(patientService.Get(), "PatientId", "Name", selectedPatient);
            ViewBag.DoctorId = new SelectList(doctorService.GetActive(), "DoctorId", "Name", selectedDoctor);
        }
    }
}
