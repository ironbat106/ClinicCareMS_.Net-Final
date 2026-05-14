using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class AppointmentRepo
    {
        ClinicCareDbContext db;

        public AppointmentRepo(ClinicCareDbContext db)
        {
            this.db = db;
        }

        public bool Create(Appointment appointment)
        {
            db.Appointments.Add(appointment);
            return db.SaveChanges() > 0;
        }

        public List<Appointment> Get()
        {
            return db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
        }

        public Appointment? Get(int id)
        {
            return db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.AppointmentId == id);
        }

        public Appointment? GetPlain(int id)
        {
            return db.Appointments.Find(id);
        }

        public bool Update(Appointment appointment)
        {
            var exobj = GetPlain(appointment.AppointmentId);
            if (exobj == null) return false;
            db.Entry(exobj).CurrentValues.SetValues(appointment);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exobj = GetPlain(id);
            if (exobj == null) return false;
            db.Appointments.Remove(exobj);
            return db.SaveChanges() > 0;
        }

        public bool ChangeStatus(int id, string status)
        {
            var exobj = GetPlain(id);
            if (exobj == null) return false;
            exobj.Status = status;
            return db.SaveChanges() > 0;
        }

        public bool IsSlotTaken(int doctorId, DateTime dateTime, int? ignoreAppointmentId = null)
        {
            return db.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate == dateTime &&
                a.Status != "Cancelled" &&
                (!ignoreAppointmentId.HasValue || a.AppointmentId != ignoreAppointmentId.Value));
        }

        public List<Appointment> GetByDateRange(DateTime from, DateTime to)
        {
            return db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate >= from && a.AppointmentDate <= to)
                .OrderBy(a => a.AppointmentDate)
                .ToList();
        }

        public List<Appointment> GetDoctorSchedule(int doctorId, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1).AddTicks(-1);
            return db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate >= start && a.AppointmentDate <= end)
                .OrderBy(a => a.AppointmentDate)
                .ToList();
        }

        public List<Appointment> GetOverdueAlerts()
        {
            return db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate < DateTime.Now && (a.Status == "Pending" || a.Status == "Confirmed"))
                .OrderBy(a => a.AppointmentDate)
                .ToList();
        }
    }
}
