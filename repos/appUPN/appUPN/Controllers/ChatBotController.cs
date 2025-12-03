using Microsoft.AspNetCore.Mvc;
using appUPN.Services;

namespace appUPN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        private readonly ILogger<ChatBotController> _logger;

        public ChatBotController(IChatBotService chatBotService, ILogger<ChatBotController> logger)
        {
            _chatBotService = chatBotService;
            _logger = logger;
        }

        [HttpPost("message")]
        public async Task<IActionResult> EnviarMensaje([FromBody] ChatRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new { error = "El mensaje no puede estar vacío" });
                }

                var respuesta = await _chatBotService.EnviarMensajeAsync(request.Message, request.SessionId);

                return Ok(new { message = respuesta });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el chatbot");
                return StatusCode(500, new { error = "Error al procesar el mensaje" });
            }
        }

        [HttpGet("history/{sessionId}")]
        public async Task<IActionResult> ObtenerHistorial(string sessionId)
        {
            try
            {
                var historial = await _chatBotService.ObtenerHistorialAsync(sessionId);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial");
                return StatusCode(500, new { error = "Error al obtener historial" });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
    }
}
