using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICustomerDAO:IBasicDB<Customer>
    {
        Customer GetCustomerByUsername(string username);
    }
}
