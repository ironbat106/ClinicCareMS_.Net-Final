using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class DoctorService
    {
        DoctorRepo repo;
        Mapper mapper;

        public DoctorService(DoctorRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }

        public List<DoctorDTO> Get()
        {
            var data = repo.Get();
            return mapper.Map<List<DoctorDTO>>(data);
        }

        public List<DoctorDTO> GetActive()
        {
            var data = repo.GetActive();
            return mapper.Map<List<DoctorDTO>>(data);
        }

        public DoctorDTO? Get(int id)
        {
            var data = repo.Get(id);
            return mapper.Map<DoctorDTO>(data);
        }

        public bool Create(DoctorDTO dto)
        {
            if (repo.EmailExists(dto.Email))
            {
                throw new Exception("This doctor email already exists.");
            }

            var data = mapper.Map<Doctor>(dto);
            return repo.Create(data);
        }

        public bool Update(DoctorDTO dto)
        {
            if (repo.EmailExists(dto.Email, dto.DoctorId))
            {
                throw new Exception("This doctor email already exists.");
            }

            var data = mapper.Map<Doctor>(dto);
            return repo.Update(data);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
