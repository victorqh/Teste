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

                // Obtener productos disponibles de la base de datos
                var productosDisponibles = await ObtenerProductosDisponiblesAsync();

                // Obtener historial para contexto
                var historial = await ObtenerHistorialAsync(sessionId, 5);
                var contexto = await ConstruirContextoAsync(historial, mensaje, productosDisponibles);

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

        private async Task<List<Producto>> ObtenerProductosDisponiblesAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.EstaActivo)
                .OrderBy(p => p.CategoriaId)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        private async Task<string> ConstruirContextoAsync(IEnumerable<ChatMessage> historial, string mensajeActual, List<Producto> productos)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("Eres un asistente virtual para appUPN, una tienda online de tecnología en Perú.");
            sb.AppendLine("Tu rol es ayudar a los clientes con información sobre productos, stock, precios y la tienda.");
            sb.AppendLine("\nInformación de la tienda:");
            sb.AppendLine("- Precios en Soles peruanos (S/)");
            sb.AppendLine("- Envío gratis en todos los pedidos");
            sb.AppendLine("- Aceptamos pagos seguros online");
            sb.AppendLine("- Los usuarios deben registrarse para comprar");
            
            sb.AppendLine("\n=== CATÁLOGO COMPLETO DE PRODUCTOS DISPONIBLES ===");
            
            var productosPorCategoria = productos.GroupBy(p => p.Categoria?.Nombre ?? "Sin categoría");
            
            foreach (var categoria in productosPorCategoria)
            {
                sb.AppendLine($"\n📦 {categoria.Key}:");
                foreach (var producto in categoria)
                {
                    sb.AppendLine($"  • {producto.Nombre}");
                    sb.AppendLine($"    - Precio: S/ {producto.Precio:N2}");
                    if (producto.PrecioAnterior.HasValue && producto.PrecioAnterior > producto.Precio)
                    {
                        var descuento = ((producto.PrecioAnterior.Value - producto.Precio) / producto.PrecioAnterior.Value * 100);
                        sb.AppendLine($"    - Precio anterior: S/ {producto.PrecioAnterior:N2} (¡{descuento:N0}% OFF!)");
                    }
                    sb.AppendLine($"    - Stock: {(producto.Stock > 0 ? $"{producto.Stock} unidades disponibles" : "AGOTADO")}");
                    if (!string.IsNullOrEmpty(producto.Descripcion))
                    {
                        sb.AppendLine($"    - Descripción: {producto.Descripcion}");
                    }
                    if (producto.EsOferta)
                    {
                        sb.AppendLine($"    - ¡EN OFERTA! 🔥");
                    }
                }
            }
            
            sb.AppendLine("\n=== FIN DEL CATÁLOGO ===");
            sb.AppendLine("\nINSTRUCCIONES:");
            sb.AppendLine("- Si preguntan por productos, usa la información exacta del catálogo (nombres, precios, stock)");
            sb.AppendLine("- Si preguntan por disponibilidad, indica el stock exacto");
            sb.AppendLine("- Si un producto está agotado (stock 0), informa que no hay disponibilidad");
            sb.AppendLine("- Si preguntan por ofertas, menciona solo los productos que tienen 'EN OFERTA' o precio anterior");
            sb.AppendLine("- Si preguntan por precios, proporciona el precio exacto");
            sb.AppendLine("- Recomienda productos similares si el que buscan está agotado");
            sb.AppendLine("- Sé amigable, profesional y conciso");
            
            sb.AppendLine("\nConversación anterior:");
            foreach (var msg in historial.TakeLast(4))
            {
                sb.AppendLine($"{msg.Role}: {msg.Message}");
            }

            sb.AppendLine($"\nPregunta actual del usuario: {mensajeActual}");
            sb.AppendLine("\nTu respuesta (usa información real del catálogo):");

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
