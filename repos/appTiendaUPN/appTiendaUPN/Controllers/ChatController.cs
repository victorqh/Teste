using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using appTiendaUPN.Services;

namespace appTiendaUPN.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // GET: Chat
        public IActionResult Index()
        {
            return View();
        }

        // POST: Chat/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { error = "El mensaje no puede estar vacío" });
            }

            try
            {
                // Obtener userId si está autenticado
                int? userId = null;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(userIdClaim, out int parsedUserId))
                    {
                        userId = parsedUserId;
                    }
                }

                // Si no hay sessionId, generar uno nuevo
                var sessionId = string.IsNullOrEmpty(request.SessionId) 
                    ? Guid.NewGuid().ToString() 
                    : request.SessionId;

                var response = await _chatService.SendMessageAsync(request.Message, sessionId, userId);

                return Ok(new
                {
                    response = response,
                    sessionId = sessionId,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al procesar el mensaje: {ex.Message}" });
            }
        }

        // GET: Chat/History
        [HttpGet]
        public async Task<IActionResult> GetHistory(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return BadRequest(new { error = "SessionId es requerido" });
            }

            try
            {
                var history = await _chatService.GetChatHistoryAsync(sessionId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener historial: {ex.Message}" });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
    }
}
