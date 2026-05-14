using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class PatientDTO
    {
        public int PatientId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        public string? Address { get; set; }
    }
}
