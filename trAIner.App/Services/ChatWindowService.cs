using System.Diagnostics;
using trAIner.App.Models.Messages;

namespace trAIner.App.Services;

internal static class ChatWindowServiceUtils
{
    public static void AddChatWindowService(this IServiceCollection services)
    {
        services.AddSingleton<ChatWindowService>();
    }
}

internal sealed class ChatWindowService
{
    private readonly List<RequestMessage> _requests = [];
    public IReadOnlyList<RequestMessage> Requests => _requests;

    public void AppendRequest(RequestMessage request) => _requests.Add(request);

    public void AppendResponse(ResponseMessage response)
    {
        var request = _requests.LastOrDefault();

        if (request is null)
        {
            return;
        }

        request.RespondedWith(response);
    }

    private bool _firstResponse = true;
    private ResponseMessage _responseMessage = null!;
    public async Task AppendResponseStream(IAsyncEnumerable<string> tokens, Func<Task> actOnStream)
    {
        var request = _requests.LastOrDefault();

        Debug.Assert(request is not null, "Request should not be NULL");

        try
        {
            await foreach (var token in tokens)
            {
                if (_firstResponse)
                {
                    _responseMessage = new(token);
                    request.RespondedWith(_responseMessage);

                    _firstResponse = false;
                    await actOnStream.Invoke().ConfigureAwait(false);
                    continue;
                }

                _responseMessage.AppendResponseFromStream(token);
                await actOnStream.Invoke().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _responseMessage.ErrorOccured(ex.Message);
            await actOnStream.Invoke().ConfigureAwait(false);
        }

        _firstResponse = true;
    }
}
