using CoreLayer.Entities.Identity;
using CoreLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Data;
using System.Security.Claims;
using SkillBridgeDentistry1.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CoreLayer.Services;
using System.Text.Json;
using System.Net.Mail;

namespace SkillBridgeDentistry1.Controllers
{
    public class CaseRequestsController : ApiBaseController
    {
        private readonly SkillBridgeDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAIService _aiService;
        private readonly INotificationService _notificationService;

        public CaseRequestsController(SkillBridgeDbContext context,
        IWebHostEnvironment environment,
        UserManager<ApplicationUser> userManager,
        IAIService aiService, INotificationService notificationService)
        {
            _context = context;
            _env = environment;
            _userManager = userManager;
            _aiService = aiService;
            _notificationService = notificationService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "FreshGraduate")]
        [HttpPost("upload-case-request")]
        public async Task<IActionResult> UploadCaseRequest([FromForm] UploadCaseRequestDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            string imagePath = null;
            string absoluteImagePath = null;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "caseImages");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                imagePath = Path.Combine("caseImages", uniqueFileName); // relative path
                absoluteImagePath = Path.Combine(uploadsFolder, uniqueFileName); // full path

                using (var stream = new FileStream(absoluteImagePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await dto.Image.CopyToAsync(stream);
                }
            }

            // نرسل الصورة إلى AI model
            var aiResult = await _aiService.PredictDiseaseAsync(absoluteImagePath);

            // استخراج التخصص من الـ prediction (الذي أرجعه نموذج الـ AI)
            int? specialityId = null;
            if (aiResult.prediction != null)
            {
                var speciality = await _context.Specialities
                    .FirstOrDefaultAsync(s => s.Name == aiResult.prediction);

                specialityId = speciality?.SpecialityId;
            }
            var freshGrad = await _context.freashGrad.FirstOrDefaultAsync(f => f.freashGradId == userId);
            if (freshGrad == null)
                return BadRequest("Not Authorized As FreshGraduate Dentist");

            // إنشاء كائن الحالة مع SpecialityId المستخرج
            var caseRequest = new CaseRequest
            {
                Diagnoses = dto.Diagnois,
                SpecialityId = specialityId,
                ImagePath = imagePath,
                freashGradId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.CaseRequests.Add(caseRequest);
            await _context.SaveChangesAsync();

            string treatment = null;

            // ✅ تعديل المقارنة لتكون مع النسبة الصحيحة
            if (aiResult.confidence >= 0.95)
            {
                if (!string.IsNullOrWhiteSpace(aiResult.prediction))
                {
                    var httpClient = new HttpClient();
                    var treatmentUrl = $"https://dentalaiapp.onrender.com/treatment/{Uri.EscapeDataString(aiResult.prediction)}";

                    try
                    {
                        var response = await httpClient.GetAsync(treatmentUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Received JSON from FastAPI: {json}");

                            var treatmentData = JsonSerializer.Deserialize<TreatmentResponse>(json, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            treatment = treatmentData?.Treatment;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching treatment from FastAPI: {ex.Message}");
                    }
                }
            }
            else
            {
                // إذا كانت النسبة أقل من 95%، إرسال الحالة للاستشاريين
                if (specialityId != null)
                {
                    var speciality = await _context.Specialities
                        .FirstOrDefaultAsync(s => s.SpecialityId == specialityId);

                    if (speciality != null)
                    {
                        var consultants = await _context.Consultants
                            .Where(c => c.DepartmentId == speciality.DepartmentId)
                            .ToListAsync();
                        var imageUrl = $"{Request.Scheme}://{Request.Host}/caseImages/{Path.GetFileName(imagePath)}";

                        foreach (var consultant in consultants)
                        {
                            var caseConsultant = new CaseConsultant
                            {
                                CaseRequestId = caseRequest.CaseRequestId,
                                ConsultantId = consultant.ConsultantId,
                            };

                            _context.CaseConsultants.Add(caseConsultant);
                            await _context.SaveChangesAsync();  // Get CaseConsultantId

                            var message = $"Diagnoses : {dto.Diagnois}. " + $"Click The Link To See Case: {imageUrl}";

                            // Send notification with CaseRequestId and CaseConsultantId
                            await _notificationService.CreateNotificationAsync(
                                consultant.UserId,
                                "New Case Request",
                                message,
                                caseRequest.CaseRequestId,
                                caseConsultant.CaseConsultantId
                            );

                            
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest("speciality Not Found");
                    }
                }
                else
                {
                    return BadRequest("Can't Found Consultants For This Speciality");
                }
            }

            // حفظ نتيجة AI
            var newAIResult = new AIResult
            {
                DiseaseName = aiResult.prediction,
                Confidence = aiResult.confidence,
                Treatment = treatment ?? "Not Found Treatment ",
                CaseRequestId = caseRequest.CaseRequestId,
                CreatedAt = DateTime.UtcNow,
                SpecialityId = specialityId
            };

            _context.AIResults.Add(newAIResult);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Case Request uploaded and analyzed by AI successfully",
                caseRequestId = caseRequest.CaseRequestId,
                prediction = aiResult.prediction,
                confidence = aiResult.confidence,
                treatment = treatment??"Sent Case For Specific Consultant"
            });
        }
    }
}






