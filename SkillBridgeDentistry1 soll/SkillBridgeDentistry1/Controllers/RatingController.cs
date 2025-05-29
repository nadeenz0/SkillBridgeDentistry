using CoreLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using SkillBridgeDentistry1.Dtos;
using System;
using System.Security.Claims;

namespace SkillBridgeDentistry1.Controllers
{

    public class RatingController : ApiBaseController
    {
        private readonly SkillBridgeDbContext _context;

        public RatingController(SkillBridgeDbContext context)
        {
            _context = context;
            _context=context;
        }

        [Authorize(Roles = "FreshGraduate")]
        [HttpPost("rate-consultant")]
        public async Task<IActionResult> RateConsultant([FromBody] RateDto dto)
        {
            var caseConsultant = await _context.CaseConsultants
                .FirstOrDefaultAsync(cc => cc.CaseConsultantId == dto.CaseConsultantId);

            if (caseConsultant == null)
                return NotFound("No CaseConsultant Found");

            caseConsultant.Rating = dto.Rate;

            // تحديث متوسط تقييم الاستشاري في جدول Consultant
            var rates = await _context.CaseConsultants
                .Where(cc => cc.ConsultantId == caseConsultant.ConsultantId && cc.Rating.HasValue)
                .Select(cc => cc.Rating.Value)
                .ToListAsync();

            var averageRating = rates.Any() ? Convert.ToDecimal(rates.Average()) : 0;

            var consultant = await _context.Consultants
                .FirstOrDefaultAsync(c => c.ConsultantId == caseConsultant.ConsultantId);

            if (consultant != null)
            {
                consultant.Rating = averageRating;

                // تأكيد إنه مضاف للتراك
                _context.Consultants.Update(consultant);
            }

            await _context.SaveChangesAsync();

            return Ok("Rating Saved Successfully");
        }





        [HttpGet("consultants-for-rating/{caseRequestId}")]
        public async Task<IActionResult> GetConsultantsForRating(int caseRequestId)
        {
            var consultants = await _context.CaseConsultants
                .Include(cc => cc.Consultant)
                .ThenInclude(c => c.User)
                .Where(cc => cc.CaseRequestId == caseRequestId && cc.Diagnosis != null &&cc.Treatment != null)
                .Select(cc => new
                {
                    CaseConsultantId = cc.CaseConsultantId,
                    ConsultantId = cc.ConsultantId,   
                }).ToListAsync();

            return Ok(consultants);
        }

    }
}


