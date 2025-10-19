using FORUM.Contracts;

namespace FORUM.ViewClass
{
    public class MainViewModel
    {
        public UserResponse? User { get; set; }
        public IEnumerable<PostResponse>? Posts { get; set; }
    }
}
