using System.ComponentModel.DataAnnotations;

namespace SkillBridgeDentistry1.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string Email { get; set; } = null!;
        public string Otp { get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }= null!;

        [Required]
        [Compare("NewPassword",ErrorMessage ="The Password and confirmation password don't match")]
        public string ConfirmPassword { get; set; }= null!;
        
    }
}

