using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using trAIner.App.Services;

namespace trAIner.App.Components.Pages;

public partial class ChatWindow
{
    [Inject] ChatWindowService ChatWindowService { get; set; } = null!;
    [Inject] IJSRuntime JS { get; set; } = null!;

    public async Task AppendRequestAsync(string request)
    {
        ChatWindowService.AppendRequest(new(request));
        await InvokeAsync(StateHasChanged);
        await JS.InvokeVoidAsync("scrollToBottom");
    }

    public async Task AppendResponseAsync(string response)
    {
        ChatWindowService.AppendResponse(new(response));
        await InvokeAsync(StateHasChanged);
        await JS.InvokeVoidAsync("scrollToBottom");
    }

    public async Task AppendResponseStreamAsync(IAsyncEnumerable<string> tokens)
    {
        await ChatWindowService.AppendResponseStream(tokens, async () =>
        {
            await Task.WhenAll(
                InvokeAsync(StateHasChanged), 
                JS.InvokeVoidAsync("scrollToBottom", "chatContainer").AsTask())
            .ConfigureAwait(false);
        });
    }
}
