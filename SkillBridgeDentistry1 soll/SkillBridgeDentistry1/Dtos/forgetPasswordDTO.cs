using System.ComponentModel.DataAnnotations;

namespace SkillBridgeDentistry1.Dtos
{
    public class forgetPasswordDTO

    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
