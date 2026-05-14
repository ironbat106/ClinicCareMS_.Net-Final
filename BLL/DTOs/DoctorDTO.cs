using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
