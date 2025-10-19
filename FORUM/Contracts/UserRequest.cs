using DomainModels.Enums;

namespace FORUM.Contracts
{
    public record UserRequest(string name, string password, Role role);
}
