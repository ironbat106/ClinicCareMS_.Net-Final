using DAL.EF;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class DoctorRepo
    {
        ClinicCareDbContext db;

        public DoctorRepo(ClinicCareDbContext db)
        {
            this.db = db;
        }

        public bool Create(Doctor doctor)
        {
            db.Doctors.Add(doctor);
            return db.SaveChanges() > 0;
        }

        public List<Doctor> Get()
        {
            return db.Doctors.OrderBy(d => d.Name).ToList();
        }

        public List<Doctor> GetActive()
        {
            return db.Doctors.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
        }

        public Doctor? Get(int id)
        {
            return db.Doctors.Find(id);
        }

        public bool Update(Doctor doctor)
        {
            var exobj = Get(doctor.DoctorId);
            if (exobj == null) return false;
            db.Entry(exobj).CurrentValues.SetValues(doctor);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exobj = Get(id);
            if (exobj == null) return false;
            db.Doctors.Remove(exobj);
            return db.SaveChanges() > 0;
        }

        public bool EmailExists(string? email, int? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return db.Doctors.Any(d => d.Email == email && (!ignoreId.HasValue || d.DoctorId != ignoreId.Value));
        }
    }
}
