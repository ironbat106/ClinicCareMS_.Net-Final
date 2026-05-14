using BLL.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a patient.")]
        public int PatientId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a doctor.")]
        public int DoctorId { get; set; }

        [Required]
        [FutureOrToday]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        [Required]
        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        [Range(0, 100000)]
        public decimal Fee { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
