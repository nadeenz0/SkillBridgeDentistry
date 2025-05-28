namespace SkillBridgeDentistry1.Dtos
{
    public class NotificationDto
    {

        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime SentAt { get; set; }
        public int? CaseRequestId { get; set; }
        public int? CaseConsultantId { get; set; }
    }
}
