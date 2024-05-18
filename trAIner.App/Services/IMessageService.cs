namespace trAIner.App.Services;

public interface IMessageService
{
    Task SendRequestMessageAsync(string message, CancellationToken ct = default);
}
