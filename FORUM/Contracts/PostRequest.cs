namespace FORUM.Contracts
{
    public record PostRequest(string title, string content, IFormFile file);
}
