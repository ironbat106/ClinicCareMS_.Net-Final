using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Filters;

namespace MVCApp.Controllers
{
    [LoginRequired]
    public class PatientsController : Controller
    {
        PatientService service;

        public PatientsController(PatientService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View(service.Get());
        }

        public IActionResult Details(int id)
        {
            var data = service.Get(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PatientDTO());
        }

        [HttpPost]
        public IActionResult Create(PatientDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Create(dto);
                    TempData["Msg"] = "Patient created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = service.Get(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(PatientDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Update(dto);
                    TempData["Msg"] = "Patient updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(dto);
        }

        [AdminOnly]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = service.Get(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [AdminOnly]
        [HttpPost]
        public IActionResult Delete(int id, string decision)
        {
            if (decision == "Yes")
            {
                try
                {
                    service.Delete(id);
                    TempData["Msg"] = "Patient deleted successfully.";
                }
                catch
                {
                    TempData["Err"] = "Delete failed. This patient may have appointments.";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
