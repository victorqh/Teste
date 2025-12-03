using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using appTiendaUPN.Data;
using appTiendaUPN.Models;
using Microsoft.EntityFrameworkCore;

namespace appTiendaUPN.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductoService _productoService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ChatService(
            ApplicationDbContext context,
            IProductoService productoService,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _productoService = productoService;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> SendMessageAsync(string userMessage, string sessionId, int? userId = null)
        {
            // Guardar mensaje del usuario
            var userChatMessage = new ChatMessage
            {
                UserId = userId,
                SessionId = sessionId,
                Role = "user",
                Message = userMessage,
                CreatedAt = DateTime.UtcNow
            };
            _context.ChatMessages.Add(userChatMessage);
            await _context.SaveChangesAsync();

            // Procesar mensaje con IA
            var botResponse = await ProcessWithGeminiAsync(userMessage, sessionId);

            // Guardar respuesta del bot
            var botChatMessage = new ChatMessage
            {
                UserId = userId,
                SessionId = sessionId,
                Role = "assistant",
                Message = botResponse,
                CreatedAt = DateTime.UtcNow
            };
            _context.ChatMessages.Add(botChatMessage);
            await _context.SaveChangesAsync();

            return botResponse;
        }

        private async Task<string> ProcessWithGeminiAsync(string userMessage, string sessionId)
        {
            try
            {
                var apiKey = _configuration["GeminiApiKey"];
                
                if (string.IsNullOrEmpty(apiKey) || apiKey == "TU_API_KEY_AQUI")
                {
                    return "⚠️ El chatbot aún no está configurado. Por favor, agrega tu API Key de Google Gemini en appsettings.json";
                }

                // Obtener historial de conversación
                var history = await GetChatHistoryAsync(sessionId);
                
                // Construir contexto del sistema
                var systemPrompt = @"Eres un asistente virtual amigable de 'Tienda UPN', una tienda en línea peruana que vende:
- Laptops (HP, Lenovo, Dell, ASUS, MacBook, MSI)
- Smartphones (iPhone, Samsung, Xiaomi, Google Pixel, OnePlus, Motorola)
- Tablets (iPad, Samsung Galaxy Tab, Lenovo, Huawei)
- Televisores (Samsung, LG, Sony, TCL, Hisense)
- Electrodomésticos (Lavadoras, Refrigeradoras, Microondas, Aspiradoras, Licuadoras)
- Audio (AirPods, Sony, JBL, Bose, Samsung Buds)
- Gaming (PlayStation, Xbox, Nintendo Switch, accesorios gamer)
- Accesorios tecnológicos

Tu trabajo es:
✅ Ayudar a los clientes a encontrar productos
✅ Responder preguntas sobre productos, precios y especificaciones
✅ Recomendar productos según sus necesidades
✅ Ser amable, profesional y útil
✅ Hablar en español de forma natural
✅ Usar emojis ocasionalmente para ser más amigable

Importante:
- Los precios están en soles peruanos (S/)
- Si preguntan por un producto específico, intenta ayudarles a encontrar algo similar
- Si no sabes algo, sé honesto y ofrece alternativas
- Sé breve pero informativo";

                // Detectar si está buscando productos
                var productContext = await GetProductContextAsync(userMessage);

                // Construir el prompt completo
                var fullPrompt = $"{systemPrompt}\n\n";
                
                if (!string.IsNullOrEmpty(productContext))
                {
                    fullPrompt += $"Información de productos disponibles:\n{productContext}\n\n";
                }

                // Agregar historial reciente (últimos 5 mensajes)
                var recentHistory = history.TakeLast(5).ToList();
                foreach (var msg in recentHistory)
                {
                    fullPrompt += $"{(msg.Role == "user" ? "Cliente" : "Asistente")}: {msg.Message}\n";
                }

                fullPrompt += $"Cliente: {userMessage}\nAsistente:";

                // Llamar a Gemini API
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = fullPrompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,
                        maxOutputTokens = 500,
                        topP = 0.95,
                        topK = 40
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"❌ Error al conectar con Gemini: {response.StatusCode}. Verifica tu API Key.";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonDocument.Parse(responseContent);
                
                var candidates = jsonResponse.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    var contentProperty = firstCandidate.GetProperty("content");
                    var parts = contentProperty.GetProperty("parts");
                    if (parts.GetArrayLength() > 0)
                    {
                        var textResponse = parts[0].GetProperty("text").GetString();
                        return textResponse ?? "Lo siento, no pude generar una respuesta.";
                    }
                }

                return "Lo siento, no pude procesar tu mensaje. ¿Podrías reformularlo?";
            }
            catch (Exception ex)
            {
                return $"❌ Error: {ex.Message}. Por favor, intenta de nuevo.";
            }
        }

        private async Task<string> GetProductContextAsync(string userMessage)
        {
            var lowerMessage = userMessage.ToLower();
            
            // Detectar palabras clave de búsqueda
            var keywords = new[] { "busco", "quiero", "necesito", "muestra", "recomienda", "precio", "costo", "laptop", "celular", "teléfono", "tablet", "tv", "oferta", "barato", "gaming" };
            
            if (!keywords.Any(k => lowerMessage.Contains(k)))
            {
                return string.Empty;
            }

            try
            {
                // Intentar buscar productos relevantes
                IEnumerable<Producto> productos = new List<Producto>();

                if (lowerMessage.Contains("oferta") || lowerMessage.Contains("descuento") || lowerMessage.Contains("promocion"))
                {
                    productos = await _productoService.GetOfertasAsync();
                }
                else if (lowerMessage.Contains("laptop") || lowerMessage.Contains("computador"))
                {
                    productos = await _productoService.GetProductosPorCategoriaAsync(1); // Laptops
                }
                else if (lowerMessage.Contains("celular") || lowerMessage.Contains("teléfono") || lowerMessage.Contains("telefono") || lowerMessage.Contains("smartphone"))
                {
                    productos = await _productoService.GetProductosPorCategoriaAsync(2); // Smartphones
                }
                else if (lowerMessage.Contains("tablet"))
                {
                    productos = await _productoService.GetProductosPorCategoriaAsync(3); // Tablets
                }
                else if (lowerMessage.Contains("gaming") || lowerMessage.Contains("juego") || lowerMessage.Contains("consola"))
                {
                    productos = await _productoService.GetProductosPorCategoriaAsync(7); // Gaming
                }
                else
                {
                    // Búsqueda general
                    var searchTerms = lowerMessage.Split(' ').Where(w => w.Length > 3);
                    foreach (var term in searchTerms)
                    {
                        var results = await _productoService.BuscarProductosAsync(term);
                        if (results.Any())
                        {
                            productos = results;
                            break;
                        }
                    }
                }

                if (!productos.Any())
                {
                    return string.Empty;
                }

                // Limitar a 5 productos más relevantes
                var topProducts = productos.Take(5).ToList();
                var productInfo = new StringBuilder();
                productInfo.AppendLine("Productos disponibles:");
                
                foreach (var p in topProducts)
                {
                    var precioAnterior = p.PrecioAnterior.HasValue ? $" (Antes: S/ {p.PrecioAnterior:N2})" : "";
                    var oferta = p.EsOferta ? " 🔥 OFERTA" : "";
                    productInfo.AppendLine($"- {p.Nombre}: S/ {p.Precio:N2}{precioAnterior}{oferta} | Stock: {p.Stock} unidades");
                }

                return productInfo.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(string sessionId)
        {
            return await _context.ChatMessages
                .Where(m => m.SessionId == sessionId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
