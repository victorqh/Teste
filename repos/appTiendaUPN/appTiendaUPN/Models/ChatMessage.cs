namespace appTiendaUPN.Models
{
    public class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public int? UserId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "user" o "assistant"
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navegación
        public User? User { get; set; }
    }
}
