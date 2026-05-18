using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class AuthService
    {
        AppUserRepo repo;

        public AuthService(AppUserRepo repo)
        {
            this.repo = repo;
        }

        public AppUser Login(LoginDTO dto)
        {
            var hashedPassword = GetMd5(dto.Password);
            var user = repo.GetByUserName(dto.UserName);

            if (user == null || user.Password != hashedPassword)
            {
                throw new Exception("Invalid username or password.");
            }

            user.Token = Guid.NewGuid().ToString();
            repo.Update(user);
            return user;
        }

        public bool Register(RegDTO dto)
        {
            var existing = repo.GetByUserName(dto.UserName);
            if (existing != null)
            {
                throw new Exception("Username already exists.");
            }

            var user = new AppUser()
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                Password = GetMd5(dto.Password),
                Role = "Staff",
                Token = null,
                IsActive = true
            };

            return repo.Create(user);
        }

        public int GetUserType(AppUser user)
        {
            if (user.Role == "Admin") return 1;
            return 2;
        }

        string GetMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
