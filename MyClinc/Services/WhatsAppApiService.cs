using System.Text;
using System.Text.Json;

namespace MyClinc.Services
{
    public interface IWhatsAppApiService
    {
        Task<bool> SendMessage(string phoneNumber, string message);
        Task<bool> SendTemplateMessage(string phoneNumber, string templateName, Dictionary<string, string> parameters);
        Task<bool> VerifyWebhook(string mode, string token, string challenge);
    }

    public class WhatsAppApiService : IWhatsAppApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WhatsAppApiService> _logger;

        public WhatsAppApiService(HttpClient httpClient, IConfiguration configuration, ILogger<WhatsAppApiService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            // Configure HTTP client
            var accessToken = _configuration["WhatsApp:AccessToken"];
            var phoneNumberId = _configuration["WhatsApp:PhoneNumberId"];
            var apiVersion = _configuration["WhatsApp:ApiVersion"];
            var baseUrl = _configuration["WhatsApp:BaseUrl"];
            
            _httpClient.BaseAddress = new Uri($"{baseUrl}/{apiVersion}/{phoneNumberId}/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        }

        public async Task<bool> SendMessage(string phoneNumber, string message)
        {
            try
            {
                var requestBody = new
                {
                    messaging_product = "whatsapp",
                    to = phoneNumber,
                    type = "text",
                    text = new { body = message }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("messages", content);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Message sent successfully to {PhoneNumber}", phoneNumber);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send message to {PhoneNumber}. Status: {Status}, Error: {Error}", 
                        phoneNumber, response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending WhatsApp message to {PhoneNumber}", phoneNumber);
                return false;
            }
        }

        public async Task<bool> SendTemplateMessage(string phoneNumber, string templateName, Dictionary<string, string> parameters)
        {
            try
            {
                var components = new List<object>();
                
                if (parameters.Any())
                {
                    var templateParams = parameters.Select(p => new { type = "text", text = p.Value }).ToArray();
                    components.Add(new
                    {
                        type = "body",
                        parameters = templateParams
                    });
                }

                var requestBody = new
                {
                    messaging_product = "whatsapp",
                    to = phoneNumber,
                    type = "template",
                    template = new
                    {
                        name = templateName,
                        language = new { code = "ar" },
                        components = components
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("messages", content);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Template message sent successfully to {PhoneNumber}", phoneNumber);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send template message to {PhoneNumber}. Status: {Status}, Error: {Error}", 
                        phoneNumber, response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending WhatsApp template message to {PhoneNumber}", phoneNumber);
                return false;
            }
        }

        public async Task<bool> VerifyWebhook(string mode, string token, string challenge)
        {
            try
            {
                var verifyToken = _configuration["WhatsApp:WebhookVerifyToken"];
                
                if (mode == "subscribe" && token == verifyToken)
                {
                    _logger.LogInformation("Webhook verified successfully");
                    return true;
                }
                
                _logger.LogWarning("Webhook verification failed. Mode: {Mode}, Token: {Token}", mode, token);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying webhook");
                return false;
            }
        }
    }
} 