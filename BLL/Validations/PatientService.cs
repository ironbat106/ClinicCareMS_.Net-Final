using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class PatientService
    {
        PatientRepo repo;
        Mapper mapper;

        public PatientService(PatientRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }

        public List<PatientDTO> Get()
        {
            var data = repo.Get();
            return mapper.Map<List<PatientDTO>>(data);
        }

        public PatientDTO? Get(int id)
        {
            var data = repo.Get(id);
            return mapper.Map<PatientDTO>(data);
        }

        public bool Create(PatientDTO dto)
        {
            if (repo.EmailExists(dto.Email))
            {
                throw new Exception("This patient email already exists.");
            }

            var data = mapper.Map<Patient>(dto);
            return repo.Create(data);
        }

        public bool Update(PatientDTO dto)
        {
            if (repo.EmailExists(dto.Email, dto.PatientId))
            {
                throw new Exception("This patient email already exists.");
            }

            var data = mapper.Map<Patient>(dto);
            return repo.Update(data);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
