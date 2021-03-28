using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IFlightsTicketsHistoryDAO
    {
        void Add(Flight flight);
        void Add(Ticket ticket);

    }
}
