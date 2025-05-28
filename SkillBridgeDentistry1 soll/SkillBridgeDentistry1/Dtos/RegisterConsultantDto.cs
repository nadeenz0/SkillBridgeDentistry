using System.ComponentModel.DataAnnotations;

namespace SkillBridgeDentistry1.Dtos
{
    public class RegisterConsultantDto
    {
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
        public IFormFile ResumeLink { get; set; }
        public int YearsOfExperience { get; set; }
        public string Department { get; set; }
        public string ShortBiography { get; set; }
        public IFormFile Photo { get; set; }
    }
}
