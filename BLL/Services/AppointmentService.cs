using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class AppointmentService
    {
        AppointmentRepo appointmentRepo;
        DoctorRepo doctorRepo;
        PatientRepo patientRepo;
        Mapper mapper;

        string[] statuses = { "Pending", "Confirmed", "Completed", "Cancelled" };

        public AppointmentService(AppointmentRepo appointmentRepo, DoctorRepo doctorRepo, PatientRepo patientRepo)
        {
            this.appointmentRepo = appointmentRepo;
            this.doctorRepo = doctorRepo;
            this.patientRepo = patientRepo;
            mapper = MapperConfig.GetMapper();
        }

        public List<AppointmentDTO> Get()
        {
            var data = appointmentRepo.Get();
            var res = mapper.Map<List<AppointmentDTO>>(data);
            return res;
        }

        public AppointmentDTO? Get(int id)
        {
            var data = appointmentRepo.Get(id);
            var res = mapper.Map<AppointmentDTO>(data);
            return res;
        }

        public bool Create(AppointmentDTO dto)
        {
            CheckAppointment(dto, null);

            dto.Status = "Pending";
            dto.CreatedAt = DateTime.Now;

            var data = mapper.Map<Appointment>(dto);
            var res = appointmentRepo.Create(data);
            return res;
        }

        public bool Update(AppointmentDTO dto)
        {
            CheckAppointment(dto, dto.AppointmentId);

            var data = mapper.Map<Appointment>(dto);
            var res = appointmentRepo.Update(data);
            return res;
        }

        public bool Delete(int id)
        {
            var res = appointmentRepo.Delete(id);
            return res;
        }

        public bool ChangeStatus(int id, string status)
        {
            if (!statuses.Contains(status))
            {
                throw new Exception("Invalid appointment status.");
            }

            var res = appointmentRepo.ChangeStatus(id, status);
            return res;
        }

        public AppointmentReportDTO GetReport(DateTime? fromDate, DateTime? toDate)
        {
            var from = (fromDate ?? DateTime.Today).Date;
            var to = (toDate ?? DateTime.Today).Date.AddDays(1).AddTicks(-1);

            var data = appointmentRepo.GetByDateRange(from, to);
            var res = mapper.Map<List<AppointmentDTO>>(data);

            var report = new AppointmentReportDTO();
            report.FromDate = from;
            report.ToDate = to.Date;
            report.TotalAppointments = data.Count;
            report.Pending = data.Count(a => a.Status == "Pending");
            report.Confirmed = data.Count(a => a.Status == "Confirmed");
            report.Completed = data.Count(a => a.Status == "Completed");
            report.Cancelled = data.Count(a => a.Status == "Cancelled");
            report.TotalCompletedFee = data.Where(a => a.Status == "Completed").Sum(a => a.Fee);
            report.Appointments = res;

            return report;
        }

        public List<AppointmentDTO> GetDoctorSchedule(int doctorId, DateTime date)
        {
            var data = appointmentRepo.GetDoctorSchedule(doctorId, date);
            var res = mapper.Map<List<AppointmentDTO>>(data);
            return res;
        }

        public List<AppointmentDTO> GetOverdueAlerts()
        {
            var data = appointmentRepo.GetOverdueAlerts();
            var res = mapper.Map<List<AppointmentDTO>>(data);
            return res;
        }

        public DashboardDTO GetDashboard()
        {
            var today = appointmentRepo.GetByDateRange(DateTime.Today, DateTime.Today.AddDays(1).AddTicks(-1));
            var overdue = appointmentRepo.GetOverdueAlerts();
            var todayList = mapper.Map<List<AppointmentDTO>>(today);

            var dashboard = new DashboardDTO();
            dashboard.TotalDoctors = doctorRepo.Get().Count;
            dashboard.TotalPatients = patientRepo.Get().Count;
            dashboard.TodayAppointments = today.Count;
            dashboard.OverdueAppointments = overdue.Count;
            dashboard.TodayList = todayList;

            return dashboard;
        }

        private void CheckAppointment(AppointmentDTO dto, int? ignoreId)
        {
            if (patientRepo.Get(dto.PatientId) == null)
            {
                throw new Exception("Selected patient was not found.");
            }

            if (doctorRepo.Get(dto.DoctorId) == null)
            {
                throw new Exception("Selected doctor was not found.");
            }

            if (appointmentRepo.IsSlotTaken(dto.DoctorId, dto.AppointmentDate, ignoreId))
            {
                throw new Exception("This doctor already has an appointment at this time.");
            }
        }
    }
}