using DAL.EF;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class AppUserRepo
    {
        ClinicCareDbContext db;

        public AppUserRepo(ClinicCareDbContext db)
        {
            this.db = db;
        }

        public AppUser? GetByUserName(string userName)
        {
            return db.AppUsers.FirstOrDefault(u => u.UserName == userName && u.IsActive == true);
        }

        public AppUser? Get(int id)
        {
            return db.AppUsers.Find(id);
        }

        public List<AppUser> Get()
        {
            return db.AppUsers.ToList();
        }

        public bool Create(AppUser user)
        {
            db.AppUsers.Add(user);
            return db.SaveChanges() > 0;
        }

        public bool Update(AppUser user)
        {
            var exobj = Get(user.UserId);
            if (exobj == null) return false;

            db.Entry(exobj).CurrentValues.SetValues(user);
            return db.SaveChanges() > 0;
        }
    }
}
