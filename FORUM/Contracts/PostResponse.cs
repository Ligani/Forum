namespace FORUM.Contracts
{
    public record PostResponse(Guid id, string title, string content, string filepath, DateTime dateTime);
}
