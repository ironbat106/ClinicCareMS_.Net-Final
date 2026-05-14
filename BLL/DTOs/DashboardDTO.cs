using System.Collections.Generic;

namespace BLL.DTOs
{
    public class DashboardDTO
    {
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TodayAppointments { get; set; }
        public int OverdueAppointments { get; set; }
        public List<AppointmentDTO> TodayList { get; set; } = new List<AppointmentDTO>();
    }
}
