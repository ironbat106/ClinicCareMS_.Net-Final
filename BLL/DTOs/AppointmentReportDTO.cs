using System;
using System.Collections.Generic;

namespace BLL.DTOs
{
    public class AppointmentReportDTO
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalAppointments { get; set; }
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
        public decimal TotalCompletedFee { get; set; }
        public List<AppointmentDTO> Appointments { get; set; } = new List<AppointmentDTO>();
    }
}
