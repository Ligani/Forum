using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Models
{
    public class Post
    {
        public const int MAX_TITLE_LENGTH = 30;
        public const int MAX_CONTENT_LENGTH = 300;


        private Post(Guid id, Guid user_Id, string title, string content, DateTime created, string filePath) 
        {
            Id = id;
            User_Id = user_Id;
            Title = title;
            Content = content;
            Created = created;
            FilePath = filePath;
        }
        public Guid Id { get; set; }
        public Guid User_Id { get; set; }
        public string Title {  get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string FilePath { get; set; }

        public static (Post post, string error) CreatePost(Guid id, Guid user_Id, string title, string content, DateTime created, string filePath)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
            {
                error += $"title can not be empty or longer than {MAX_TITLE_LENGTH}\n";
            }

            if (string.IsNullOrEmpty(content) || content.Length > MAX_CONTENT_LENGTH)
            {
                error += $"content can not be empty or longer than {MAX_CONTENT_LENGTH}\n";
            }


            var post = new Post(id, user_Id, title, content, created, filePath);

            return (post, error);
        }
    }
}
