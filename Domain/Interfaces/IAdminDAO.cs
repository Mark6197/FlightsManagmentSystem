using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAdminDAO : IBasicDB<Administrator>
    {
        Administrator GetAdministratorByUsernameAndPassword(string username, string password);

    }
}
