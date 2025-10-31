using DomainModels.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Models
{
    public class User
    {
        public const int MAX_NAME_LENGTH = 14;
        public const int MIN_NAME_LENGTH = 4;

        public const int MAX_PASSWORD_LENGTH = 16;
        public const int MIN_PASSWORD_LENGTH = 7;
        private User(Guid id, string name, Role roleOfUser, string hashPassword, string? about)
        {
            Id = id;
            Name = name;
            RoleOfUser = roleOfUser;
            HashPassword = hashPassword;
            About = about;
        }

        public Guid Id { get; }
        public string Name { get; }
        public Role RoleOfUser { get; }
        public string HashPassword { get; set; }
        public string? About {  get; set; }

        public static (User user, string error) CreateUser(Guid id, string name, Role role, string password, string? about)
        {
            var error = string.Empty;

            if (name.Length > MAX_NAME_LENGTH || string.IsNullOrEmpty(name) || name.Length <= MIN_NAME_LENGTH)
            {
                error += "Имя не может быть пустым или длиннее чем 14 символов/короче чем 4 символов\n";
            }
            if (password.Length > MAX_PASSWORD_LENGTH || password.Length < MIN_PASSWORD_LENGTH || string.IsNullOrEmpty(name))
            {
                error += "Пароль не может быть пустым или длиннее чем 16 символов/короче чем 7 символов\n"; ;
            }
            var user = new User(id, name, role, password, about);

            return (user, error);
        }

    }
}
