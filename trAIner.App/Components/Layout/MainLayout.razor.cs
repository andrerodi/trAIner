using Microsoft.AspNetCore.Components;
using MudBlazor;
using trAIner.App.Components.Pages;
using trAIner.App.Services;

namespace trAIner.App.Components.Layout;
public partial class MainLayout : IDisposable
{
    [Inject] private ITrAInerService AiService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private ChatWindow _chatWindow = null!;

    private bool _drawerOpen = false;
    private bool _isLoading = false;
    private string _prooompt = "Outlined";

    protected override void OnInitialized()
    {
        AiService.IsLoading += AiSerivceIsLoading;
    }

    private async Task RequestAsync(string prooompt)
    {
        await _chatWindow.AppendRequestAsync(prooompt).ConfigureAwait(false);
        _prooompt = string.Empty;
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        await _chatWindow.AppendResponseStreamAsync(AiService.StreamRequestAsync(prooompt)).ConfigureAwait(false);
    }

    private void AiSerivceIsLoading(bool obj)
    {
        _isLoading = obj;
        InvokeAsync(StateHasChanged);
    }

    private void DrawerToggle() => _drawerOpen = !_drawerOpen;

    private readonly MudTheme _jetbrainsMonoFont = new()
    {
        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = ["JetBrains Mono", "Roboto", "Helvetica", "Arial", "sans-serif"]
            }
        }
    };

    public void Dispose()
    {
        AiService.IsLoading -= AiSerivceIsLoading;
    }
}
