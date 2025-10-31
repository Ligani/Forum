using DomainModels.Models;

namespace Logics.Interfaces
{
    public interface IPasswordHasherService
    {
        bool CheckHashPassword(User user, string password);
        User HashPassword(string password, User user);
    }
}