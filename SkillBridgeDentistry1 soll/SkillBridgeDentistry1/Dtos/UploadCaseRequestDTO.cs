using System.ComponentModel.DataAnnotations;

namespace SkillBridgeDentistry1.Dtos
{
    public class UploadCaseRequestDTO
    {
        public string? Diagnois { get; set; }= string.Empty;

        public IFormFile? Image { get; set; } 

    }
}
