using CoreLayer.Entities;
using CoreLayer.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer;
using SkillBridgeDentistry1.Dtos;
using System.Security.Claims;

namespace SkillBridgeDentistry1.Controllers
{
    
    public class RespondToCase : ApiBaseController
    {
        private readonly SkillBridgeDbContext _context;
        private readonly INotificationService _notificationService;

        public RespondToCase(SkillBridgeDbContext context,INotificationService notificationService)
        {
            _context=context;
            _notificationService=notificationService;
        }

        [Authorize(Roles = "Consultant")]
        [HttpPost("respond-to-case")]
        public async Task<IActionResult> RespondToCaseFromConsultant([FromBody] ConsultantResponseDto dto)
        {
            var caseConsultant = await _context.CaseConsultants
                .Include(cc => cc.CaseRequest)
                .Include(cc => cc.Consultant)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(cc => cc.CaseConsultantId== dto.CaseConsultantId);

            if (caseConsultant == null)
                return NotFound("No Case Found");

            // حفظ الرد
            caseConsultant.Diagnosis = dto.Diagnosis;
            caseConsultant.Treatment = dto.Treatment;
            caseConsultant.ReplyDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            BackgroundJob.Schedule<NotificationScheduler>(
             scheduler => scheduler.SendRateNotifications(dto.CaseRequestId),
            TimeSpan.FromHours(48));

            // إرسال إشعار للطبيب حديث التخرج
            var freshGraduateUserId = caseConsultant.CaseRequest.freashGradId;
            var consultantName = caseConsultant.Consultant.User.FullName;

            var notificationTitle = $"Response From  {consultantName}";
            var notificationBody = $"The Diagnose is : {dto.Diagnosis}.  Proposed Treatment: {dto.Treatment}.";

            await _notificationService.CreateNotificationAsync(
            freshGraduateUserId,
            notificationTitle,
            notificationBody,
            dto.CaseRequestId,
            dto.CaseConsultantId
            );

            

            return Ok("Response Is Sent Successfully");
        }

        [HttpGet("case-responses/{caseRequestId}")]
        public async Task<IActionResult> GetCaseConsultantResponses(int caseRequestId)
        {
            // نتحقق إن الحالة موجودة
            var caseRequest = await _context.CaseRequests
                .FirstOrDefaultAsync(c => c.CaseRequestId== caseRequestId);

            if (caseRequest == null)
            {
                return NotFound("No Case Found");
            }

            // نجيب الردود من الاستشاريين على الحالة دي
            var responses = await _context.CaseConsultants
                .Include(cc => cc.Consultant)
                    .ThenInclude(c => c.User)
.Where(cc => cc.CaseRequestId == caseRequestId && cc.Diagnosis != null && cc.Treatment != null)
                .OrderByDescending(cc => cc.Consultant.Rating)
                .Select(cc => new
                {
                    ConsultantName = cc.Consultant.User.FullName,
                    ConsultantPhoto = cc.Consultant.ProfilePicturePath,
                    Biography = cc.Consultant.ShortBiography,
                    Diagnosis = cc.Diagnosis,
                    Treatment = cc.Treatment,
                    Rate = cc.Consultant.Rating
                })
                .ToListAsync();

            if (!responses.Any())
            {
                return Ok("No Responses Available Now ");
            }

            return Ok(responses);
        }
        [HttpGet("GetCaseConsultantsForCaseRequest/{caseRequestId}")]
        public async Task<IActionResult> GetCaseConsultantsForCaseRequest(int caseRequestId)
        {
            var caseConsultants = await _context.CaseConsultants
                .Include(cc => cc.Consultant)
                .ThenInclude(c => c.User)
                .Where(cc => cc.CaseRequestId == caseRequestId)
                .Select(cc => new
                {
                    CaseConsultantId = cc.CaseConsultantId,
                    ConsultantName = cc.Consultant.User.FullName,
                    ConsultantPhoto = cc.Consultant.ProfilePicturePath,
                    ShortBiography = cc.Consultant.ShortBiography,
                    Rate = cc.Consultant.Rating,
                    ConsultantId = cc.ConsultantId
                })
                .OrderByDescending(cc => cc.Rate)
                .ToListAsync();

            if (!caseConsultants.Any())
            {
                return NotFound("There is No consultant For This Case");
            }

            return Ok(caseConsultants);
        }


    }
}
