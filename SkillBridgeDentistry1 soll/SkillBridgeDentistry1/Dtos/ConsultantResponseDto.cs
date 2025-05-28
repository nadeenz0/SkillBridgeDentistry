namespace SkillBridgeDentistry1.Dtos
{
    public class ConsultantResponseDto
    {
        public int CaseConsultantId { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        
        public int CaseRequestId { get; set; }
    }
}
