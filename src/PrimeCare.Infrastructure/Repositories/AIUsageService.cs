using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.AiEntites;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Shared.Dtos.AiDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Application.Services.Implementations
{
    public class AIUsageService : IAIUsageService
    {
        private readonly PrimeCareContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AIUsageService> _logger;

        public AIUsageService(
            PrimeCareContext context,
            IConfiguration configuration,
            ILogger<AIUsageService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AIUsageStatusDto> GetUsageStatusAsync(string userId)
        {
            var dailyLimit = _configuration.GetValue<int>("MedicalAI:DailyUsageLimit", 100);
            var todayUsage = await GetTodayUsageCountAsync(userId);
            var allowed = todayUsage < dailyLimit;

            var message = allowed
                ? todayUsage >= dailyLimit - 2
                    ? $"Warning: You have used {todayUsage}/{dailyLimit} of your daily AI consultations. Limit is near."
                    : $"You have used {todayUsage}/{dailyLimit} consultations today."
                : "Daily AI consultation limit reached. Please try again tomorrow.";

            return new AIUsageStatusDto
            {
                Allowed = allowed,
                CurrentCount = todayUsage,
                DailyLimit = dailyLimit,
                Message = message
            };
        }

        public async Task<bool> CheckAIUsageLimitAsync(string userId)
        {
            try
            {
                var dailyLimit = _configuration.GetValue<int>("MedicalAI:DailyUsageLimit", 100);
                var todayUsage = await GetTodayUsageCountAsync(userId);

                _logger.LogInformation($"User {userId} - Daily limit: {dailyLimit}, Today's usage: {todayUsage}");

                // Return true if user is within limit (usage is less than limit)
                return todayUsage < dailyLimit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking AI usage limit for user {userId}");
                // In case of error, be conservative and deny access
                return false;
            }
        }

        public async Task UpdateAIUsageAsync(string userId)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var existingUsage = await _context.AIUsageTrackings
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Date == today);

                if (existingUsage == null)
                {
                    _context.AIUsageTrackings.Add(new AIUsageTracking
                    {
                        UserId = userId,
                        Date = today,
                        RequestCount = 1,
                        LastRequestTime = DateTime.UtcNow
                    });

                    _logger.LogInformation($"Created new AI usage record for user {userId}");
                }
                else
                {
                    existingUsage.RequestCount++;
                    existingUsage.LastRequestTime = DateTime.UtcNow;

                    _logger.LogInformation($"Updated AI usage for user {userId}, new count: {existingUsage.RequestCount}");
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating AI usage for user {userId}");
                throw; // Re-throw to let the caller handle the error
            }
        }

        public async Task<int> GetTodayUsageCountAsync(string userId)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var usage = await _context.AIUsageTrackings
                    .AsNoTracking() // Add this for better performance when only reading
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Date == today);

                var count = usage?.RequestCount ?? 0;
                _logger.LogDebug($"Retrieved usage count for user {userId}: {count}");

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting today's usage count for user {userId}");
                // Return a high number to be safe in case of database errors
                return int.MaxValue;
            }
        }

        // Additional method to get remaining usage for the user
        public async Task<int> GetRemainingUsageAsync(string userId)
        {
            try
            {
                var dailyLimit = _configuration.GetValue<int>("MedicalAI:DailyUsageLimit", 10);
                var todayUsage = await GetTodayUsageCountAsync(userId);

                return Math.Max(0, dailyLimit - todayUsage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting remaining usage for user {userId}");
                return 0;
            }
        }

        // Method to reset usage for testing purposes (optional)
        public async Task ResetUserUsageAsync(string userId)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var existingUsage = await _context.AIUsageTrackings
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Date == today);

                if (existingUsage != null)
                {
                    _context.AIUsageTrackings.Remove(existingUsage);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Reset AI usage for user {userId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resetting AI usage for user {userId}");
                throw;
            }
        }
    }
}