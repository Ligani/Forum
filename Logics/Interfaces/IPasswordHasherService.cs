using DomainModels.Models;

namespace Logics.Interfaces
{
    public interface IPasswordHasherService
    {
        bool CheckHashPassword(User_ user, string password);
        User_ HashPassword(string password, User_ user);
    }
}