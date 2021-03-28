
using Domain.Interfaces;

namespace BL.LoginService
{
    public interface ILoginService
    {
        bool TryLogin(string userName, string password, out ILoginToken token, out FacadeBase facade);
    }
}
