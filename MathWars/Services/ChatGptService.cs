using MathWars.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MathWars.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<ChatGptService> _logger;

        public ChatGptService(HttpClient httpClient, IConfiguration configuration, ILogger<ChatGptService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _logger = logger;
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var requestContent = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 100
            };

            var requestJson = JsonSerializer.Serialize(requestContent);
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            try
            {
                _logger.LogInformation("Sending request to OpenAI API.");
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<JsonElement>(responseJson);
                    var responseText = responseObject.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    _logger.LogInformation("Received response from OpenAI API: {responseText}", responseText);
                    return responseText;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error response from OpenAI API: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while sending request to OpenAI API.");
            }

            return null;
        }
    }
}
