using API.Areas.Admin.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace API.Models.MyHelper
{
    public interface IGoogleService

    {
        Boolean CheckGoogle(string SecretKey, string Token,  int flagCaptCha = 0);
        bool CheckGoogleV2(string secretKey, string? token, int idCoQuan = 1, int flagCaptCha = 0);
    }
    public class GoogleService : IGoogleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Boolean CheckGoogle(string SecretKey, string Token,int flagCaptCha = 0)
        {

            Boolean flag = false;
           
            if (flagCaptCha == 1)
            {
                string link = "https://www.google.com/recaptcha/api/siteverify?secret=" + SecretKey + "&response=" + Token;

                var options = new RestClientOptions(link)
                {
                    ThrowOnAnyError = true,                    
                };
                var client = new RestClient(options);
                var request = new RestRequest(link);
                var response = client.Get(request);

                API.Models.Google google = JsonConvert.DeserializeObject<API.Models.Google>(response.Content);

                if (google.success == true)
                {
                    flag = true;
                }

            }
            else
            {
                flag = true;
            }
            return flag;

        }

        // Add by Tayd - reCAPTCHA v2 sites PYN
        public GoogleService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }



        public bool CheckGoogleV2(string secretKey, string? token, int idCoQuan = 1, int flagCaptCha = 0)
        {
            if (flagCaptCha == 0)
                return false;  // Skip verification


            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("HttpContext is not available.");

            var quotaService = httpContext.RequestServices.GetRequiredService<IRecaptchaQuotaService>();

            // Check quota first
            bool isExceeded = quotaService.IsQuotaExceeded(idCoQuan);
            if (isExceeded)
            {
                // Log warning if needed (inject ILogger for production)
                return true;  // Skip verify, allow request (avoid extra fees)
            }

            // Skip if no token
            if (string.IsNullOrEmpty(token))
                return false;

            // Verify with Google
            string url = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}";

            using var httpClient = new HttpClient();
            var response = httpClient.GetStringAsync(url).Result;

            var googleResponse = JsonSerializer.Deserialize<GoogleResponse>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            bool success = googleResponse?.Success ?? false;

            if (success)
            {
                quotaService.IncrementUsage(idCoQuan);  // Increment only on success
            }

            return success;
        }


        public class GoogleResponse
        {
            public bool Success { get; set; }
            public string? ChallengeTs { get; set; }
            public string? Hostname { get; set; }
            public string? ErrorCodes { get; set; }  // Can be array, but string for simple
        }

        // End Add by Tayd - reCAPTCHA v2 sites PYN
    }
}
