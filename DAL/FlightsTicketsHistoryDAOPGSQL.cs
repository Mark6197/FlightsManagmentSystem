using Domain.Entities;
using Domain.Interfaces;
using Npgsql;


namespace DAL
{
    public class FlightsTicketsHistoryDAOPGSQL : IFlightsTicketsHistoryDAO
    {
        public void Add(Flight flight)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_add_flight_history";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_original_id", flight.Id),
                    new NpgsqlParameter("_airline_company_id", flight.AirlineCompany.Id),
                    new NpgsqlParameter("_origin_country_id", flight.OriginCountryId),
                    new NpgsqlParameter("_destination_country_id", flight.DestinationCountryId),
                    new NpgsqlParameter("_departure_time", flight.DepartureTime),
                    new NpgsqlParameter("_landing_time", flight.LandingTime),
                    new NpgsqlParameter("_remaining_tickets", flight.RemainingTickets)
                });
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var id = command.ExecuteScalar();
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);

            }
        }

        public void Add(Ticket ticket)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_add_ticket_history";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_original_id", ticket.Id),
                    new NpgsqlParameter("_flight_id", ticket.Flight.Id),
                    new NpgsqlParameter("_customer_id", ticket.Customer.Id)
                });
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var id = command.ExecuteScalar();
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }
    }
}
