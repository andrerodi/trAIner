using trAIner.App.Models.Messages;

namespace trAIner.App.Models.Topics;

internal sealed class Topic
{
    public Ulid Id { get; private set; } = Ulid.NewUlid();
    public string Name { get; private set; }
    public DateTimeOffset CreatedAd { get; private set; } = DateTimeOffset.UtcNow;
    public List<RequestMessage> Messages { get; private set; } = [];

    public void AddMessage(RequestMessage message) => Messages.Add(message);

    public Topic(string name)
    {
        Name = name;
    }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Topic() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
