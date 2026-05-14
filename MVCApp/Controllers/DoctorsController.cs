using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Filters;

namespace MVCApp.Controllers
{
    [LoginRequired]
    [AdminOnly]
    public class DoctorsController : Controller
    {
        DoctorService service;

        public DoctorsController(DoctorService service)
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
            return View(new DoctorDTO());
        }

        [HttpPost]
        public IActionResult Create(DoctorDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Create(dto);
                    TempData["Msg"] = "Doctor created successfully.";
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
        public IActionResult Edit(DoctorDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Update(dto);
                    TempData["Msg"] = "Doctor updated successfully.";
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
        public IActionResult Delete(int id)
        {
            var data = service.Get(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(int id, string decision)
        {
            if (decision == "Yes")
            {
                try
                {
                    service.Delete(id);
                    TempData["Msg"] = "Doctor deleted successfully.";
                }
                catch
                {
                    TempData["Err"] = "Delete failed. This doctor may have appointments.";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
