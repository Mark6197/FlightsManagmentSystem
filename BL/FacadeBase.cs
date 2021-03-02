using Domain.Interfaces;

namespace BL
{
    public abstract class FacadeBase
    {
        protected IAirlineDAO _airlineDAO;
        protected ICountryDAO _countryDAO;
        protected ICustomerDAO _customerDAO;
        protected IAdminDAO _adminDAO;
        protected IUserDAO _userDAO;
        protected IFlightDAO _flightDAO;
        protected ITicketDAO _ticketDAO;
    }
}
