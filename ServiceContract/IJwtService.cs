using UserManagement.Authetication;
using UserManagement.DTO;

namespace UserManagement.ServiceContract
{
    public interface IJwtService
    {
        AutheticationResponse CreateJwtToken(AppUser user);
    }
}
