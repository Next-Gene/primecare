using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Shared.Dtos.AiDtos;
using System.Security.Claims;

namespace PrimeCare.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Move authorization to class level since most endpoints require it
    public class PrimeAiController : ControllerBase
    {
        private readonly IMedicalAIService _medicalAIService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAIUsageService _aiUsageService;
        private readonly ILogger<PrimeAiController> _logger;

        public PrimeAiController(
            IMedicalAIService medicalAIService,
            UserManager<ApplicationUser> userManager,
            IAIUsageService aiUsageService,
            ILogger<PrimeAiController> logger)
        {
            _medicalAIService = medicalAIService;
            _userManager = userManager;
            _aiUsageService = aiUsageService;
            _logger = logger;
        }

        [HttpPost("ask")]
        public async Task<ActionResult<MedicalAIResponse>> AskPrimeAi([FromBody] MedicalQueryRequest request)
        {
            try
            {
                // Input validation
                if (request == null)
                {
                    return BadRequest("Request cannot be null");
                }

                if (string.IsNullOrWhiteSpace(request.Query))
                {
                    return BadRequest("Query cannot be empty");
                }

                // Get and validate user
                var userId = GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(userId))
                {
                    _logger.LogWarning("JWT is missing userId claim");
                    return Unauthorized("Invalid token. UserId claim is missing");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("No user found with id {UserId}", userId);
                    return Unauthorized("User not found");
                }

                // Check usage limits (combined check)
                var usageStatus = await _aiUsageService.GetUsageStatusAsync(userId);
                if (!usageStatus.Allowed)
                {
                    _logger.LogInformation("AI usage limit reached for user {UserId}", userId);
                    return BadRequest(usageStatus.Message);
                }

                // Process the medical query
                _logger.LogInformation("Processing medical query for user {UserId}", userId);
                var response = await _medicalAIService.GetMedicalAssistanceAsync(request.Query, userId);

                // Update usage tracking
                await _aiUsageService.UpdateAIUsageAsync(userId);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument in medical query for user {UserId}", GetCurrentUserId());
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt for user {UserId}", GetCurrentUserId());
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                var userId = GetCurrentUserId();
                _logger.LogError(ex, "Error processing medical query for userId {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request");
            }
        }

        [HttpGet("usage-status")]
        public async Task<ActionResult<AIUsageStatusDto>> GetUsageStatus()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized("Invalid token");
                }

                var usageStatus = await _aiUsageService.GetUsageStatusAsync(userId);
                return Ok(usageStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting usage status for user {UserId}", GetCurrentUserId());
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving usage status");
            }
        }

        [HttpGet("emergency-contacts")]
        [AllowAnonymous] // Emergency contacts should be accessible without authentication
        public ActionResult<EmergencyContactsResponse> GetEmergencyContacts()
        {
            try
            {
                return Ok(new EmergencyContactsResponse
                {
                    EmergencyNumber = "123", // الإسعاف
                    PoisonControl = "105",   // الخط الساخن لوزارة الصحة
                    MentalHealthCrisis = "08008880700", // الخط الساخن للدعم النفسي من وزارة الصحة
                    Message = "If you are facing a medical emergency, please call the emergency services immediately. / لو بتواجه حالة طارئة، اتصل بالإسعاف فورًا."
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving emergency contacts");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving emergency contacts");
            }
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public ActionResult HealthCheck()
        {
            return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// Helper method to get current user ID from claims
        /// </summary>
        private string? GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}