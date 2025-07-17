using Microsoft.AspNetCore.Mvc;
using MyClinc.Services;
using System.Text.Json;

namespace MyClinc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppBotController : ControllerBase
    {
        private readonly IWhatsAppBotService _whatsAppBotService;

        public WhatsAppBotController(IWhatsAppBotService whatsAppBotService)
        {
            _whatsAppBotService = whatsAppBotService;
        }

        // Webhook endpoint for WhatsApp Business API
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] WhatsAppWebhookRequest request)
        {
            try
            {
                if (request?.Entry?.FirstOrDefault()?.Changes?.FirstOrDefault()?.Value?.Messages?.FirstOrDefault() is var message)
                {
                    var phoneNumber = message.From;
                    var messageText = message.Text?.Body;

                    if (!string.IsNullOrEmpty(messageText))
                    {
                        await ProcessMessage(phoneNumber, messageText);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error processing webhook: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Webhook verification endpoint
        [HttpGet("webhook")]
        public async Task<IActionResult> VerifyWebhook([FromQuery] string mode, [FromQuery] string token, [FromQuery] string challenge)
        {
            try
            {
                var isValid = await _whatsAppBotService.VerifyWebhook(mode, token, challenge);
                
                if (isValid)
                {
                    return Ok(challenge);
                }
                
                return BadRequest("Invalid verification token");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying webhook: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Manual message processing endpoint (for testing)
        [HttpPost("process-message")]
        public async Task<IActionResult> ProcessMessage([FromBody] ProcessMessageRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Message))
                {
                    return BadRequest("Phone number and message are required");
                }

                await ProcessMessage(request.PhoneNumber, request.Message);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Send welcome message endpoint
        [HttpPost("send-welcome")]
        public async Task<IActionResult> SendWelcome([FromBody] SendWelcomeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PhoneNumber))
                {
                    return BadRequest("Phone number is required");
                }

                var result = await _whatsAppBotService.SendWelcomeMessage(request.PhoneNumber);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private async Task ProcessMessage(string phoneNumber, string message)
        {
            // Check if this is a new conversation or continuation
            if (IsNewConversation(message))
            {
                await _whatsAppBotService.SendWelcomeMessage(phoneNumber);
                return;
            }

            // Process based on the message content
            if (message == "1")
            {
                await _whatsAppBotService.SendFormInstructions(phoneNumber, "consultation");
            }
            else if (message == "2")
            {
                await _whatsAppBotService.SendFormInstructions(phoneNumber, "doctor");
            }
            else if (message == "3")
            {
                await _whatsAppBotService.SendFormInstructions(phoneNumber, "partnership");
            }
            else
            {
                // Process form data based on current session
                await ProcessFormMessage(phoneNumber, message);
            }
        }

        private bool IsNewConversation(string message)
        {
            // Check if the message indicates a new conversation
            var newConversationKeywords = new[] { "مرحبا", "السلام عليكم", "hello", "hi", "start", "ابدأ" };
            return newConversationKeywords.Any(keyword => 
                message.ToLower().Contains(keyword.ToLower()));
        }

        private async Task ProcessFormMessage(string phoneNumber, string message)
        {
            // This would typically check the user's current session state
            // For now, we'll try to process it as a consultation form
            await _whatsAppBotService.ProcessConsultationForm(phoneNumber, message);
        }
    }

    // Request models
    public class WhatsAppWebhookRequest
    {
        public List<WebhookEntry> Entry { get; set; } = new();
    }

    public class WebhookEntry
    {
        public List<WebhookChange> Changes { get; set; } = new();
    }

    public class WebhookChange
    {
        public WebhookValue Value { get; set; } = new();
    }

    public class WebhookValue
    {
        public List<WhatsAppMessage> Messages { get; set; } = new();
    }

    public class WhatsAppMessage
    {
        public string From { get; set; } = "";
        public string Id { get; set; } = "";
        public string Timestamp { get; set; } = "";
        public MessageText? Text { get; set; }
    }

    public class MessageText
    {
        public string Body { get; set; } = "";
    }

    public class ProcessMessageRequest
    {
        public string PhoneNumber { get; set; } = "";
        public string Message { get; set; } = "";
    }

    public class SendWelcomeRequest
    {
        public string PhoneNumber { get; set; } = "";
    }
} 