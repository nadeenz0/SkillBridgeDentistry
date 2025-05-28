using System.ComponentModel.DataAnnotations;

namespace SkillBridgeDentistry1.Dtos
{
    public class RegisterFreshGraduateDto
    {
        public string FullName { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
        public int YearOfGraduation { get; set; }
        public string University { get; set; }
        public string Department { get; set; }
    }
}
