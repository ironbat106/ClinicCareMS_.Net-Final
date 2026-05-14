using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;

namespace BLL
{
    public class MapperConfig
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Doctor, DoctorDTO>().ReverseMap();
            cfg.CreateMap<Patient, PatientDTO>().ReverseMap();

            cfg.CreateMap<Appointment, AppointmentDTO>()
                .ForMember(d => d.DoctorName, opt => opt.MapFrom(s => s.Doctor.Name))
                .ForMember(d => d.PatientName, opt => opt.MapFrom(s => s.Patient.Name));

            cfg.CreateMap<AppointmentDTO, Appointment>();
        });

        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }
    }
}
