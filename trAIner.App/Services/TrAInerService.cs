using ErrorOr;
using Microsoft.SemanticKernel;
using System.Runtime.CompilerServices;

namespace trAIner.App.Services;

internal static class OpenAiApiServiceType
{
    public static void AddOpenAiApiService(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(trAIner), configure =>
        {
            configure.BaseAddress = new Uri(new Uri("http://dev:11434"), "v1/chat/completions");
        });

        //Uri uri = new("http://dev:11434");
        //var client = new OllamaApiClient(uri, "llama3");

        services.AddTransient<ITrAInerService, TrAInerService>();
        services.AddTransient<Kernel>(sp =>
        {
            IHttpClientFactory factory = sp.GetRequiredService<IHttpClientFactory>();

            return Kernel.CreateBuilder()
                .AddOpenAIChatCompletion("llama3", "ollama", httpClient: factory.CreateClient(nameof(trAIner)))
                .Build();
        });
    }
}

public interface ITrAInerService
{
    Action<bool>? IsLoading { get; set; }

    Task<ErrorOr<string>> RequestAsync(string pro0Ompt, CancellationToken ct = default);
    IAsyncEnumerable<string> StreamRequestAsync(string pro0Ompt, CancellationToken ct = default);
}

internal sealed class TrAInerService(Kernel kernel) : ITrAInerService
{
    public Action<bool>? IsLoading { get; set; }

    public async Task<ErrorOr<string>> RequestAsync(string pro0Ompt, CancellationToken ct = default)
    {
        try
        {
            var res = await kernel.InvokePromptAsync(pro0Ompt, cancellationToken: ct);

            return res.GetValue<string>()!;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }
    }

    public async IAsyncEnumerable<string> StreamRequestAsync(string pro0Ompt, [EnumeratorCancellation] CancellationToken ct = default)
    {
        IsLoading?.Invoke(true);
        await foreach (var res in kernel.InvokePromptStreamingAsync(pro0Ompt, cancellationToken: ct))
        {
            yield return res.ToString();
        }
        IsLoading?.Invoke(false);
    }
}
