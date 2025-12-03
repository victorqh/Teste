namespace appUPN.Services
{
    public interface IChatBotService
    {
        Task<string> EnviarMensajeAsync(string mensaje, string sessionId);
        Task<IEnumerable<Models.ChatMessage>> ObtenerHistorialAsync(string sessionId, int cantidad = 10);
    }
}
