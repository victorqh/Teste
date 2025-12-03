namespace appTiendaUPN.Services
{
    public interface IChatService
    {
        Task<string> SendMessageAsync(string userMessage, string sessionId, int? userId = null);
        Task<IEnumerable<Models.ChatMessage>> GetChatHistoryAsync(string sessionId);
    }
}
