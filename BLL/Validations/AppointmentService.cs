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
            return mapper.Map<List<AppointmentDTO>>(data);
        }

        public AppointmentDTO? Get(int id)
        {
            var data = appointmentRepo.Get(id);
            return mapper.Map<AppointmentDTO>(data);
        }

        public bool Create(AppointmentDTO dto)
        {
            CheckAppointment(dto, null);
            dto.Status = "Pending";
            dto.CreatedAt = DateTime.Now;

            var data = mapper.Map<Appointment>(dto);
            return appointmentRepo.Create(data);
        }

        public bool Update(AppointmentDTO dto)
        {
            CheckAppointment(dto, dto.AppointmentId);
            var data = mapper.Map<Appointment>(dto);
            return appointmentRepo.Update(data);
        }

        public bool Delete(int id)
        {
            return appointmentRepo.Delete(id);
        }

        public bool ChangeStatus(int id, string status)
        {
            if (!statuses.Contains(status))
            {
                throw new Exception("Invalid appointment status.");
            }
            return appointmentRepo.ChangeStatus(id, status);
        }

        public AppointmentReportDTO GetReport(DateTime? fromDate, DateTime? toDate)
        {
            var from = (fromDate ?? DateTime.Today).Date;
            var to = (toDate ?? DateTime.Today).Date.AddDays(1).AddTicks(-1);

            var data = appointmentRepo.GetByDateRange(from, to);
            var dtoList = mapper.Map<List<AppointmentDTO>>(data);

            return new AppointmentReportDTO
            {
                FromDate = from,
                ToDate = to.Date,
                TotalAppointments = data.Count,
                Pending = data.Count(a => a.Status == "Pending"),
                Confirmed = data.Count(a => a.Status == "Confirmed"),
                Completed = data.Count(a => a.Status == "Completed"),
                Cancelled = data.Count(a => a.Status == "Cancelled"),
                TotalCompletedFee = data.Where(a => a.Status == "Completed").Sum(a => a.Fee),
                Appointments = dtoList
            };
        }

        public List<AppointmentDTO> GetDoctorSchedule(int doctorId, DateTime date)
        {
            var data = appointmentRepo.GetDoctorSchedule(doctorId, date);
            return mapper.Map<List<AppointmentDTO>>(data);
        }

        public List<AppointmentDTO> GetOverdueAlerts()
        {
            var data = appointmentRepo.GetOverdueAlerts();
            return mapper.Map<List<AppointmentDTO>>(data);
        }

        public DashboardDTO GetDashboard()
        {
            var today = appointmentRepo.GetByDateRange(DateTime.Today, DateTime.Today.AddDays(1).AddTicks(-1));
            var overdue = appointmentRepo.GetOverdueAlerts();

            return new DashboardDTO
            {
                TotalDoctors = doctorRepo.Get().Count,
                TotalPatients = patientRepo.Get().Count,
                TodayAppointments = today.Count,
                OverdueAppointments = overdue.Count,
                TodayList = mapper.Map<List<AppointmentDTO>>(today)
            };
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
