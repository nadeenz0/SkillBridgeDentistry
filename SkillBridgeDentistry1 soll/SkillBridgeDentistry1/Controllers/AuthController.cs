using CoreLayer.Entities;
using CoreLayer.Entities.Identity;
using CoreLayer.Serveses;
using CoreLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer;
using SkillBridgeDentistry1.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace SkillBridgeDentistry1.Controllers
{

    public class AuthController : ApiBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly SkillBridgeDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOtpService _otpService;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, SkillBridgeDbContext context, IWebHostEnvironment env, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager,IOtpService otpService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
            _env = env;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _otpService = otpService;
        }

        [HttpPost("register-freshgraduate")]
        public async Task<IActionResult> RegisterFreshGraduate(RegisterFreshGraduateDto dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                FullName = dto.FullName,
                UserName = dto.Email.Split('@')[0],

                freashGrad = new freashGrad
                {
                    GraduationDate = dto.YearOfGraduation,
                    University = dto.University,
                    Department = dto.Department
                }

            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var freshGraduateRole = "FreshGraduate";
            if (!await _roleManager.RoleExistsAsync(freshGraduateRole))
                await _roleManager.CreateAsync(new IdentityRole(freshGraduateRole));

            await _userManager.AddToRoleAsync(user, freshGraduateRole);

            await _userManager.UpdateAsync(user);

            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            var userRoles = await _userManager.GetRolesAsync(user);


            return Ok(new
            {
                message = "success",
                user = new
                {
                    name = user.FullName,
                    email = user.Email,
                    role = userRoles.FirstOrDefault(),
                    freshGradId = user.freashGrad.freashGradId
                },
                token = token
            });
        }


        [HttpPost("register/consultant")]
        public async Task<IActionResult> RegisterConsultant([FromForm] RegisterConsultantDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // حفظ ملف السيرة الذاتية
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = $"{Guid.NewGuid()}_{dto.ResumeLink.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ResumeLink.CopyToAsync(fileStream);
            }

            string profilePictureFileName = null;
            if (dto.Photo != null)
            {
                profilePictureFileName = $"{Guid.NewGuid()}_{dto.Photo.FileName}";
                var profilePicturePath = Path.Combine(_env.WebRootPath, "uploads", profilePictureFileName);
                using (var stream = new FileStream(profilePicturePath, FileMode.Create))
                {
                    await dto.Photo.CopyToAsync(stream);
                }
            }

            // البحث عن DepartmentId بناءً على اسم التخصص
            var department = await _context.Department
                                           .FirstOrDefaultAsync(d => d.Name == dto.Department);

            if (department == null)
            {
                return BadRequest("The specified department does not exist.");
            }

            // إنشاء مستخدم جديد
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                RoleType = Enum.Parse<RoleType>("Consultant") // تحديد نوع الدور كـ "استشاري"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                // إضافة الدور "استشاري" للمستخدم
                await _userManager.AddToRoleAsync(user, "Consultant");

                // إنشاء كائن استشاري جديد وربطه بالمستخدم
                var consultant = new Consultant
                {
                    UserId = user.Id, // ربط الاستشاري بالمستخدم باستخدام UserId
                    YearsOfExperience = dto.YearsOfExperience,
                    DepartmentId = department.DepartmentId,  // ربط الاستشاري بالتخصص باستخدام DepartmentId
                    ResumeLink = uniqueFileName,
                    ShortBiography = dto.ShortBiography,
                    ProfilePicturePath = profilePictureFileName
                };

                // إضافة الاستشاري إلى قاعدة البيانات
                _context.Consultants.Add(consultant);
                await _context.SaveChangesAsync();

                //return Ok("Consultant registered successfully.");
                var token = await _tokenService.CreateTokenAsync(user, _userManager);
                var userRoles = await _userManager.GetRolesAsync(user);

                return Ok(new
                {
                    message = "success",
                    user = new
                    {
                        name = user.FullName,
                        email = user.Email,
                        role = userRoles.FirstOrDefault(),
                        consultantId = consultant.ConsultantId
                    },
                    token = token
                });
            }

            return BadRequest(result.Errors);
        }



        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO dto)
        {
            var User = await _userManager.FindByEmailAsync(dto.Email);
            if (User is null) return Unauthorized();
            var Result = await _signInManager.CheckPasswordSignInAsync(User, dto.Password, false);
            if (!Result.Succeeded) return Unauthorized();
            var userRoles = await _userManager.GetRolesAsync(User);
            var role = userRoles.FirstOrDefault();

            string? consultantId = null;
            string? freshGradId = null;

            if (role == "Consultant")
            {
                var consultant = await _context.Consultants
                                               .FirstOrDefaultAsync(c => c.UserId == User.Id);
                consultantId = consultant?.ConsultantId;
            }
            else if (role == "FreshGraduate")
            {
                var freshGrad = await _context.freashGrad
                                              .FirstOrDefaultAsync(fg => fg.freashGradId == User.Id);
                freshGradId = freshGrad?.freashGradId;
            }

            return Ok(new 
            {
                name = User.FullName,
                Email = User.Email,
                role = role,
                consultantId = consultantId,
                freshGradId = freshGradId,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });

        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] forgetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found");

            var otp = _otpService.GenerateOtp(6);
            _otpService.SaveOtp(dto.Email, otp);

            var messageBody = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee; border-radius: 10px;'>
            <h2 style='color: #333; text-align: center;'>SkillBridgeDentistry</h2>
            <p style='font-size: 18px;'>Hello!</p>
            <p style='font-size: 16px;'>Your OTP verification code is:</p>
            <h1 style='font-size: 36px; color: #007BFF; text-align: center;'>{otp}</h1>
            <p style='font-size: 14px; color: #555;'>Please use this code to verify your email.</p>
            <p style='font-size: 14px; color: #999;'>This code will expire in 10 minutes.</p>
        </div>";

            var message = new Message(
                new string[] { dto.Email },
                "Your OTP Verification Code",
                messageBody
            );

            await _emailSender.SendEmailAsync(message);

            return Ok("OTP sent successfully.");
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] OtpVerificationDto dto)
        {
            var savedOtp = _otpService.GetOtp(dto.Email);
            if (savedOtp == null || savedOtp != dto.Otp)
                return BadRequest("Invalid or expired OTP.");

            return Ok("OTP is valid.");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found");

            var savedOtp = _otpService.GetOtp(dto.Email);
            if (savedOtp == null || savedOtp != dto.Otp)
                return BadRequest("Invalid or expired OTP.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            _otpService.RemoveOtp(dto.Email);

            return Ok("Password reset successfully.");
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.freashGrad)
                .Include(u => u.Consultant)
                .ThenInclude(c => c.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var profile = new ProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
            };

            if (user.freashGrad != null)
            {
                profile.Role = role;
                profile.Data = new FreshGraduatedDentistDto
                {
                    University = user.freashGrad.University,
                    Department = user.freashGrad.Department,
                    YearsOfGraduation = user.freashGrad.GraduationDate
                };
            }
            else if (user.Consultant != null)
            {
                profile.Role = role;
                profile.Data = new ConsultantDto
                {
                    ResumeLink = user.Consultant.ResumeLink,
                    ProfilePicturePath = user.Consultant.ProfilePicturePath,
                    YearOfExperience = user.Consultant.YearsOfExperience,
                    Department = user.Consultant.Department?.Name
                };
            }
            else
            {
                profile.Role = "User";
                profile.Data = null;
            }

            return Ok(profile);
        }

    }
}


