using trAIner.App.Models.Topics;

namespace trAIner.App.Services;

internal sealed class TempMessageService : IMessageService
{
    private readonly List<Topic> _topics = [];

    public Task SendRequestMessageAsync(string message, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}
