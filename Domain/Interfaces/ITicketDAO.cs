using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface ITicketDAO : IBasicDB<Ticket>
    {
        IList<Ticket> GetTicketsByAirlineCompany(AirlineCompany airlineCompany);
    }
}
