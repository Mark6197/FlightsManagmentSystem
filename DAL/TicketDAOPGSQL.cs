using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class TicketDAOPGSQL : BasicDB<Ticket>, ITicketDAO
    {
        public override long Add(Ticket ticket)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_add_ticket";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_flight_id", ticket.Flight.Id),
                    new NpgsqlParameter("_customer_id", ticket.Customer.Id)
                });
                command.CommandType = System.Data.CommandType.StoredProcedure;

                long id = (long)command.ExecuteScalar();

                return id;

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override Ticket Get(int id)
        {
            Ticket ticket = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_ticket";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ticket = new Ticket
                    {
                        Id = (long)reader["ticket_id"],
                        Flight = new Flight
                        {
                            Id = (long)reader["flight_id"],
                            AirlineCompany = new AirlineCompany
                            {
                                Id = (long)reader["airline_company_id"],
                                Name = (string)reader["airline_company_name"],
                                CountryId = (int)reader["airline_company_country_id"]
                            },
                            OriginCountryId = (int)reader["origin_country_id"],
                            DestinationCountryId = (int)reader["destination_country_id"],
                            DepartureTime = (DateTime)reader["departure_time"],
                            LandingTime = (DateTime)reader["landing_time"],
                            RemainingTickets = (int)reader["remaining_tickets"]
                        },
                        Customer = new Customer
                        {
                            Id = (long)reader["customer_id"],
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"],
                            Address = (string)reader["address"],
                            PhoneNumber = (string)reader["phone_number"],
                            CreditCardNumber = (string)reader["credit_card_number"]
                        }
                    };
                }

                return ticket;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override IList<Ticket> GetAll()
        {
            List<Ticket> tickets = new List<Ticket>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_all_tickets";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tickets.Add(
                        new Ticket
                        {
                            Id = (long)reader["ticket_id"],
                            Flight = new Flight
                            {
                                Id = (long)reader["flight_id"],
                                AirlineCompany = new AirlineCompany
                                {
                                    Id = (long)reader["airline_company_id"],
                                    Name = (string)reader["airline_company_name"],
                                    CountryId = (int)reader["airline_company_country_id"]
                                },
                                OriginCountryId = (int)reader["origin_country_id"],
                                DestinationCountryId = (int)reader["destination_country_id"],
                                DepartureTime = (DateTime)reader["departure_time"],
                                LandingTime = (DateTime)reader["landing_time"],
                                RemainingTickets = (int)reader["remaining_tickets"]
                            },
                            Customer = new Customer
                            {
                                Id = (long)reader["customer_id"],
                                FirstName = (string)reader["first_name"],
                                LastName = (string)reader["last_name"],
                                Address = (string)reader["address"],
                                PhoneNumber = (string)reader["phone_number"],
                                CreditCardNumber = (string)reader["credit_card_number"]
                            }
                        });
                }

                return tickets;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IList<Ticket> GetTicketsByAirlineCompany(AirlineCompany airlineCompany)
        {
            List<Ticket> tickets = new List<Ticket>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_tickets_by_airline_company";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_airline_company_id", airlineCompany.Id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tickets.Add(
                        new Ticket
                        {
                            Id = (long)reader["ticket_id"],
                            Flight = new Flight
                            {
                                Id = (long)reader["flight_id"],
                                AirlineCompany = new AirlineCompany
                                {
                                    Id = (long)reader["airline_company_id"],
                                    Name = (string)reader["airline_company_name"],
                                    CountryId = (int)reader["airline_company_country_id"]
                                },
                                OriginCountryId = (int)reader["origin_country_id"],
                                DestinationCountryId = (int)reader["destination_country_id"],
                                DepartureTime = (DateTime)reader["departure_time"],
                                LandingTime = (DateTime)reader["landing_time"],
                                RemainingTickets = (int)reader["remaining_tickets"]
                            },
                            Customer = new Customer
                            {
                                Id = (long)reader["customer_id"],
                                FirstName = (string)reader["first_name"],
                                LastName = (string)reader["last_name"],
                                Address = (string)reader["address"],
                                PhoneNumber = (string)reader["phone_number"],
                                CreditCardNumber = (string)reader["credit_card_number"]
                            }
                        });
                }

                return tickets;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Remove(Ticket ticket)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_remove_ticket(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", ticket.Id));

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Update(Ticket ticket)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_update_ticket(@_id, @_flight_id, @_customer_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", ticket.Id),
                    new NpgsqlParameter("@_flight_id", ticket.Flight.Id),
                    new NpgsqlParameter("@_customer_id", ticket.Customer.Id)
                });

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }
    }
}
