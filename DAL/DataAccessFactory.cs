using DAL.EF;
using DAL.Repos;

namespace DAL
{
    public class DataAccessFactory
    {
        private readonly ClinicCareDbContext db;

        public DataAccessFactory(ClinicCareDbContext db)
        {
            this.db = db;
        }

        public AppUserRepo AppUserData()
        {
            return new AppUserRepo(db);
        }

        public DoctorRepo DoctorData()
        {
            return new DoctorRepo(db);
        }

        public PatientRepo PatientData()
        {
            return new PatientRepo(db);
        }

        public AppointmentRepo AppointmentData()
        {
            return new AppointmentRepo(db);
        }
    }
}