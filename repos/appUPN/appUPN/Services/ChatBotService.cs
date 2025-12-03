using System.Text;
using System.Text.Json;
using appUPN.Data;
using appUPN.Models;
using Microsoft.EntityFrameworkCore;

namespace appUPN.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChatBotService> _logger;
        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent";

        public ChatBotService(
            IHttpClientFactory httpClientFactory,
            AppDbContext context,
            IConfiguration configuration,
            ILogger<ChatBotService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> EnviarMensajeAsync(string mensaje, string sessionId)
        {
            try
            {
                // Guardar mensaje del usuario
                var userMessage = new ChatMessage
                {
                    SessionId = sessionId,
                    Role = "user",
                    Message = mensaje,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ChatMessages.Add(userMessage);
                await _context.SaveChangesAsync();

                // Obtener historial para contexto
                var historial = await ObtenerHistorialAsync(sessionId, 5);
                var contexto = ConstruirContexto(historial, mensaje);

                // Llamar a la API de Gemini
                var respuesta = await LlamarGeminiAsync(contexto);

                // Guardar respuesta del asistente
                var assistantMessage = new ChatMessage
                {
                    SessionId = sessionId,
                    Role = "assistant",
                    Message = respuesta,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ChatMessages.Add(assistantMessage);
                await _context.SaveChangesAsync();

                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar mensaje del chatbot");
                return "Lo siento, ocurrió un error al procesar tu mensaje. Por favor, intenta nuevamente.";
            }
        }

        private string ConstruirContexto(IEnumerable<ChatMessage> historial, string mensajeActual)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("Eres un asistente virtual para appUPN, una tienda online de tecnología en Perú.");
            sb.AppendLine("Tu rol es ayudar a los clientes con información sobre productos y la tienda.");
            sb.AppendLine("\nInformación de la tienda:");
            sb.AppendLine("- Vendemos: Laptops, Smartphones, Tablets, Televisores, Electrodomésticos, Audio, Gaming y Accesorios");
            sb.AppendLine("- Precios en Soles peruanos (S/)");
            sb.AppendLine("- Envío gratis en todos los pedidos");
            sb.AppendLine("- Aceptamos pagos seguros online");
            sb.AppendLine("- Los usuarios deben registrarse para comprar");
            sb.AppendLine("\nResponde de manera amigable, profesional y concisa.");
            sb.AppendLine("Si te preguntan por productos específicos, recomienda buscar en nuestro catálogo.");
            sb.AppendLine("\nConversación anterior:");

            foreach (var msg in historial.TakeLast(4))
            {
                sb.AppendLine($"{msg.Role}: {msg.Message}");
            }

            sb.AppendLine($"\nPregunta actual del usuario: {mensajeActual}");
            sb.AppendLine("\nTu respuesta:");

            return sb.ToString();
        }

        private async Task<string> LlamarGeminiAsync(string prompt)
        {
            try
            {
                var apiKey = _configuration["GeminiApiKey"];
                
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("API Key de Gemini no configurada");
                    return "Error de configuración. Por favor contacta al administrador.";
                }

                var client = _httpClientFactory.CreateClient();
                // Usar gemini-2.0-flash-exp (experimental, más avanzado)
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation($"Enviando request a Gemini API: {url.Substring(0, url.IndexOf("?"))}");

                var response = await client.PostAsync(url, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Gemini API Error {response.StatusCode}: {errorContent}");
                    return "Lo siento, el servicio de chat no está disponible en este momento. Por favor intenta más tarde.";
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(responseJson);

                var text = result
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "Lo siento, no pude generar una respuesta.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en LlamarGeminiAsync");
                return "Lo siento, ocurrió un error al comunicarme con el servicio de IA.";
            }
        }

        public async Task<IEnumerable<ChatMessage>> ObtenerHistorialAsync(string sessionId, int cantidad = 10)
        {
            return await _context.ChatMessages
                .Where(m => m.SessionId == sessionId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(cantidad)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
