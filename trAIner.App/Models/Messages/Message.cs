namespace trAIner.App.Models.Messages;

internal abstract class Message
{
    public Ulid Id { get; private set; } = Ulid.NewUlid();
    public string Content { get; protected set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    protected Message(string content)
    {
        Content = content;
    }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Message() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

internal sealed class RequestMessage(string content) : Message(content)
{
    public ResponseMessage? Response { get; private set; }

    public void RespondedWith(ResponseMessage msg) => Response = msg;
}

internal sealed class ResponseMessage(string content) : Message(content)
{
    public string? Error { get; private set; }

    public void ErrorOccured(string error) => Error = error;

    public void AppendResponseFromStream(string response) => Content += response;
}
