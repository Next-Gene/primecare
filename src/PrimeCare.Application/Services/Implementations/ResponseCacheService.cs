using PrimeCare.Application.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimeCare.Application.Services.Implementations
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _cacheDatabase;

        public ResponseCacheService( IConnectionMultiplexer redis)
        {
            _cacheDatabase = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            // Implementation for caching the response
             if (response == null)
            {

                return;

            }
            var options = new JsonSerializerOptions
            {

                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(response, options);
              await _cacheDatabase.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }


        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            // Implementation for retrieving the cached response

            var cachedResponse = await _cacheDatabase.StringGetAsync(cacheKey);
            if (cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse.ToString();

        }
    }
    
    }
