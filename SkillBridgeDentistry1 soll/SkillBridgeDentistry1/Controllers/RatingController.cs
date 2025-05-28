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
                .FirstOrDefaultAsync(cc => cc.CaseRequestId == dto.CaseRequestId && cc.ConsultantId == dto.ConsultantId);

            if (caseConsultant == null)
                return NotFound("No Case Or Consultant Found");

            caseConsultant.Rating = dto.Rate;

            // تحديث متوسط تقييم الاستشاري في جدول Consultant
            var consultant = await _context.Consultants.FindAsync(dto.ConsultantId);
            if (consultant != null)
            {
                var rates = await _context.CaseConsultants
                    .Where(cc => cc.ConsultantId == dto.ConsultantId && cc.Rating.HasValue)
                    .Select(cc => cc.Rating.Value)
                    .ToListAsync();

                consultant.Rating = Convert.ToDecimal(rates.Average());
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
                    ConsultantId = cc.ConsultantId,
                    Name = cc.Consultant.User.FullName
                }).ToListAsync();

            return Ok(consultants);
        }

    }
}


