using DAL.EF;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class PatientRepo
    {
        ClinicCareDbContext db;

        public PatientRepo(ClinicCareDbContext db)
        {
            this.db = db;
        }

        public bool Create(Patient patient)
        {
            db.Patients.Add(patient);
            return db.SaveChanges() > 0;
        }

        public List<Patient> Get()
        {
            return db.Patients.OrderBy(p => p.Name).ToList();
        }

        public Patient? Get(int id)
        {
            return db.Patients.Find(id);
        }

        public bool Update(Patient patient)
        {
            var exobj = Get(patient.PatientId);
            if (exobj == null) return false;
            db.Entry(exobj).CurrentValues.SetValues(patient);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exobj = Get(id);
            if (exobj == null) return false;
            db.Patients.Remove(exobj);
            return db.SaveChanges() > 0;
        }

        public bool EmailExists(string? email, int? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return db.Patients.Any(p => p.Email == email && (!ignoreId.HasValue || p.PatientId != ignoreId.Value));
        }
    }
}
