using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.AiDtos;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

public class MedicalAIService : IMedicalAIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MedicalAIService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAuditService _auditService;

    public MedicalAIService(
        HttpClient httpClient,
        ILogger<MedicalAIService> logger,
        IConfiguration configuration,
        IAuditService auditService)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        _auditService = auditService;
    }


    public async Task<MedicalAIResponse> GetMedicalAssistanceAsync(string userQuery, string userId)
    {
        try
        {
            // 1. Safety validation
            var safetyCheck = await ValidateQuerySafetyAsync(userQuery);
            if (!safetyCheck)
            {
                return new MedicalAIResponse
                {
                    Response = "I cannot provide advice on this topic. Please consult with a healthcare professional immediately.",
                    Timestamp = DateTime.UtcNow,
                    QueryId = Guid.NewGuid().ToString(),
                    IsEmergencyDetected = true,
                    Disclaimers = new List<string> { "This response was flagged for safety concerns." },
                    ConfidenceLevel = AIConfidenceLevel.High
                };
            }

            // 2. Log the interaction for audit
            await _auditService.LogAIInteractionAsync(userId, userQuery, "Medical Assistant");

            // 3. Prepare the messages for AI request
            var messages = new[]
            {
                new { role = "system", content = MedicalAIConfig.SystemPrompt },
                new { role = "user", content = $"User query: {userQuery}" }
            };

            // 4. Call Groq API with fallback mechanism
            var aiResponse = await CallGroqWithFallbackAsync(messages);

            // 5. Process and validate response
            var processedResponse = ProcessAIResponse(aiResponse, userQuery);

            // 6. Log the response
            await _auditService.LogAIResponseAsync(userId, userQuery, processedResponse.Response);

            return processedResponse;
        }
        catch (HttpRequestException httpEx) when (httpEx.Message.Contains("Daily AI consultation limit"))
        {
            _logger.LogWarning("Daily limit reached for user {UserId}", userId);
            return new MedicalAIResponse
            {
                Response = "Daily AI consultation limit reached. Please try again tomorrow.",
                Timestamp = DateTime.UtcNow,
                QueryId = Guid.NewGuid().ToString(),
                IsEmergencyDetected = false,
                ConfidenceLevel = AIConfidenceLevel.High
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Medical AI Service for user {UserId}", userId);
            return new MedicalAIResponse
            {
                Response = "I'm currently unable to provide assistance. Please consult with a healthcare professional for medical concerns.",
                Timestamp = DateTime.UtcNow,
                QueryId = Guid.NewGuid().ToString(),
                IsEmergencyDetected = false,
                ConfidenceLevel = AIConfidenceLevel.Low
            };
        }
    }

    public async Task<bool> ValidateQuerySafetyAsync(string query)
    {
        var lowerQuery = query.ToLower();

        // Check for emergency/red flag keywords
        foreach (var keyword in MedicalAIConfig.RedFlagKeywords)
        {
            if (lowerQuery.Contains(keyword.ToLower()))
            {
                _logger.LogWarning("Red flag keyword detected: {Keyword} in query: {Query}", keyword, query);
                return false;
            }
        }

        // Check for restricted topics
        foreach (var topic in MedicalAIConfig.RestrictedTopics)
        {
            if (lowerQuery.Contains(topic.ToLower()))
            {
                _logger.LogWarning("Restricted topic detected: {Topic} in query: {Query}", topic, query);
                return false;
            }
        }

        return true;
    }

    private async Task<string> CallGroqWithFallbackAsync(object[] messages)
    {
        var apiKey = _configuration["Groq:ApiKey"];
        var apiUrl = _configuration["Groq:ApiUrl"];
        var primaryModel = _configuration["Groq:PrimaryModel"];
        var fallbackModels = _configuration.GetSection("Groq:FallbackModels").Get<string[]>() ?? Array.Empty<string>();

        // Validate configuration
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("Groq API key is not configured");
            throw new InvalidOperationException("AI service is not properly configured");
        }

        if (string.IsNullOrWhiteSpace(apiUrl))
        {
            _logger.LogError("Groq API URL is not configured");
            throw new InvalidOperationException("AI service is not properly configured");
        }

        var modelsToTry = new List<string>();
        if (!string.IsNullOrWhiteSpace(primaryModel))
        {
            modelsToTry.Add(primaryModel);
        }
        modelsToTry.AddRange(fallbackModels);

        if (!modelsToTry.Any())
        {
            _logger.LogError("No AI models configured");
            throw new InvalidOperationException("No AI models are configured");
        }

        Exception? lastException = null;

        foreach (var model in modelsToTry)
        {
            try
            {
                _logger.LogInformation("Attempting to use model: {Model}", model);

                var chatRequest = new
                {
                    model = model,
                    messages = messages,
                    max_tokens = 500,
                    temperature = 0.3
                };

                // Set up HTTP client headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "PrimeCare-Medical-Assistant/1.0");

                var json = JsonSerializer.Serialize(chatRequest, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PostAsync(apiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var chatResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var message = chatResponse?.Choices?.FirstOrDefault()?.Message?.Content;

                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        _logger.LogInformation("Successfully received response from model: {Model}", model);
                        return message.Trim();
                    }
                    else
                    {
                        _logger.LogWarning("Empty response from model: {Model}", model);
                        throw new InvalidOperationException("Received empty response from AI model");
                    }
                }
                else
                {
                    _logger.LogWarning("Model {Model} failed with status {StatusCode}: {Content}",
                        model, response.StatusCode, responseContent);

                    // Check for specific error conditions
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("Invalid API key or unauthorized access");
                    }

                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        throw new HttpRequestException("Rate limit exceeded. Please try again later.");
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest &&
                        responseContent.Contains("limit", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new HttpRequestException("Daily AI consultation limit reached. Please try again tomorrow.");
                    }

                    lastException = new HttpRequestException($"API call failed: {response.StatusCode} - {responseContent}");
                }
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogWarning("Timeout while calling model: {Model}", model);
                lastException = new TimeoutException($"Request timeout for model {model}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "HTTP error while calling model: {Model}", model);
                lastException = ex;

                // Don't try other models for certain errors
                if (ex.Message.Contains("Daily AI consultation limit") ||
                    ex.Message.Contains("Rate limit exceeded"))
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unexpected error while calling model: {Model}", model);
                lastException = ex;
            }
        }

        _logger.LogError(lastException, "All AI models failed");
        throw new Exception("All available AI models failed. Please try again later.", lastException);
    }

    private bool DetermineConsultationNeed(string response, string query)
    {
        var consultationIndicators = new[]
        {
            "consult", "doctor", "healthcare provider", "medical professional",
            "serious", "persistent", "severe", "concerning", "emergency"
        };

        return consultationIndicators.Any(indicator =>
            response.Contains(indicator, StringComparison.OrdinalIgnoreCase));
    }

    private bool DetectEmergency(string response, string query)
    {
        var emergencyKeywords = new[]
        {
            "emergency", "urgent", "immediate", "call 911", "seek immediate",
            "life-threatening", "critical", "severe pain"
        };

        var lowerResponse = response.ToLower();
        var lowerQuery = query.ToLower();

        return emergencyKeywords.Any(keyword =>
            lowerResponse.Contains(keyword) || lowerQuery.Contains(keyword));
    }

    private AIConfidenceLevel DetermineConfidenceLevel(string response)
    {
        if (response.Contains("I'm not sure") || response.Contains("might be") || response.Contains("possibly"))
            return AIConfidenceLevel.Low;

        if (response.Contains("likely") || response.Contains("probably") || response.Contains("typically"))
            return AIConfidenceLevel.Medium;

        if (response.Contains("definitely") || response.Contains("certainly") || response.Contains("clearly"))
            return AIConfidenceLevel.High;

        return AIConfidenceLevel.Medium; // Default
    }

    private MedicalAIResponse ProcessAIResponse(string response, string userQuery)
    {
        // Clean up response formatting
        var cleanResponse = response.Replace("\\n", Environment.NewLine)
                                  .Replace("\\t", "\t")
                                  .Trim();

        var requiresConsultation = DetermineConsultationNeed(cleanResponse, userQuery);
        var isEmergency = DetectEmergency(cleanResponse, userQuery);
        var confidenceLevel = DetermineConfidenceLevel(cleanResponse);

        return new MedicalAIResponse
        {
            Response = cleanResponse,
            Timestamp = DateTime.UtcNow,
            QueryId = Guid.NewGuid().ToString(),
            IsEmergencyDetected = isEmergency,
            ConfidenceLevel = confidenceLevel,
            Disclaimers = new List<string>
            {
                "This information is for educational purposes only and should not replace professional medical advice.",
                "Always consult with a healthcare professional for medical concerns."
            },
            RecommendedActions = requiresConsultation ?
                new List<string> { "Consult with a healthcare professional" } :
                new List<string>()
        };
    }
}