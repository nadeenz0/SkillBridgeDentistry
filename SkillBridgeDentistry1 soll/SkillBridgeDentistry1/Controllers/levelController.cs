using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using SkillBridgeDentistry1.Dtos;

namespace SkillBridgeDentistry1.Controllers
{
    
    public class levelController : ApiBaseController
    {

        private readonly SkillBridgeDbContext _context;

        public levelController(SkillBridgeDbContext context)
        {
            _context = context;
            _context=context;
        }
        [HttpGet("consultants/levels")]
        public async Task<IActionResult> GetConsultantsLevels()
        {
            var consultants = await _context.Consultants
                .Include(c => c.User)
                .OrderByDescending(c => c.Rating) // ترتيب حسب التقييم
                .Select(c => new ConsultantLevelDto
                {
                    FullName = c.User.FullName,
                    Rate = c.Rating,
                    ShortBiography = c.ShortBiography,
                    PhotoUrl = c.ProfilePicturePath
                })
                .ToListAsync();

            return Ok(consultants);
        }

    }
}
