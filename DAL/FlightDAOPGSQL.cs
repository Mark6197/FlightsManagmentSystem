using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class FlightDAOPGSQL : BasicDB<Flight>, IFlightDAO
    {
        public override long Add(Flight flight)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_add_flight";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_airline_company_id", flight.AirlineCompany.Id),
                    new NpgsqlParameter("_origin_country_id", flight.OriginCountryId),
                    new NpgsqlParameter("_destination_country_id", flight.DestinationCountryId),
                    new NpgsqlParameter("_departure_time", flight.DepartureTime),
                    new NpgsqlParameter("_landing_time", flight.LandingTime),
                    new NpgsqlParameter("_remaining_tickets", flight.RemainingTickets)
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

        public override Flight Get(long id)
        {
            Flight flight = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flight";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    flight = new Flight
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
                    };
                }

                return flight;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override IList<Flight> GetAll()
        {
            List<Flight> flights = new List<Flight>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_all_flights";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(
                        new Flight
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
                        });
                }

                return flights;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> flights_vacancy_dict = new Dictionary<Flight, int>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_all_flights";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights_vacancy_dict.Add(
                        new Flight
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
                        }, (int)reader["remaining_tickets"]);
                }

                return flights_vacancy_dict;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IList<Flight> GetFlightsByAirlineCompany(AirlineCompany airlineCompany)
        {
            List<Flight> flights = new List<Flight>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flights_by_airline_company";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_airline_company_id", airlineCompany.Id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(
                        new Flight
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
                        });
                }

                return flights;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IList<Flight> GetFlightsByCustomer(Customer customer)
        {
            List<Flight> flights = new List<Flight>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flights_by_customer";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_customer_id", customer.Id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(
                        new Flight
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
                        });
                }

                return flights;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            List<Flight> flights = new List<Flight>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flights_by_departure_date";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_departure_date", (NpgsqlDate)departureDate));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(
                        new Flight
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
                        });
                }

                return flights;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryId)
        {
            List<Flight> flights = new List<Flight>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flights_by_destination_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_destination_country_id", countryId));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(
                        new Flight
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
                        });
                }

                return flights;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            List<Flight> flights = new List<Flight>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flights_by_landing_date";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_landing_date", (NpgsqlDate)landingDate));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(
                        new Flight
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
                        });
                }

                return flights;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryId)
        {
            List<Flight> flights = new List<Flight>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flights_by_origin_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_origin_country_id", countryId));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    flights.Add(
                        new Flight
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
                        });
                }

                return flights;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public IDictionary<Flight, List<Ticket>> GetFlightsWithTicketsAfterLanding(long seconds_after_landing)
        {
            Dictionary<Flight, List<Ticket>> flights_with_tickets = new Dictionary<Flight, List<Ticket>>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_flights_with_tickets_that_landed";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_seconds", seconds_after_landing));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();//The sp will return record for each flight and ticket togther, if the flight has no tickets, some of the fields will be null
                while (reader.Read())
                {
                    Flight flight = new Flight
                    {
                        Id = (long)reader["flight_id"]
                    };
                    if (!flights_with_tickets.ContainsKey(flight))//Check if the flight already added as key
                    {
                        flights_with_tickets.Add(//If not add the flight as key
                            new Flight
                            {
                                Id = flight.Id,
                                AirlineCompany = new AirlineCompany
                                {
                                    Id = (long)reader["airline_company_id"]
                                },
                                OriginCountryId = (int)reader["origin_country_id"],
                                DestinationCountryId = (int)reader["destination_country_id"],
                                DepartureTime = (DateTime)reader["departure_time"],
                                LandingTime = (DateTime)reader["landing_time"],
                                RemainingTickets = (int)reader["remaining_tickets"]
                            },
                            new List<Ticket>()//Creates new list to add as value to the dictionary
                            {
                                new Ticket//Create new ticket
                                {
                                    Id=(reader["ticket_id"]==DBNull.Value? 0:(long)reader["ticket_id"]),//If there are no tickets for the flight the sp will return null and the id will become  
                                    Customer=new Customer
                                    {
                                        Id=(reader["customer_id"]==DBNull.Value? 0:(long)reader["customer_id"]),//Same as the id above
                                    },
                                    Flight=new Flight
                                    {
                                        Id = flight.Id
                                    }
                                }
                            });
                    }
                    else//If the flight is already a key in the dictionary
                    {
                        flights_with_tickets[flight].Add(//Add new ticket to the list in the value
                            new Ticket
                            {
                                Id = (long)reader["ticket_id"],
                                Customer = new Customer
                                {
                                    Id = (long)reader["customer_id"]
                                },
                                Flight = new Flight
                                {
                                    Id = flight.Id
                                }
                            });
                    }
                }

                return flights_with_tickets;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }


        public override void Remove(Flight flight)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_remove_flight(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", flight.Id));

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Update(Flight flight)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_update_flight(@_id, @_airline_company_id, @_origin_country_id, @_destination_country_id, @_departure_time, @_landing_time, @_remaining_tickets)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", flight.Id),
                    new NpgsqlParameter("@_airline_company_id", flight.AirlineCompany.Id),
                    new NpgsqlParameter("@_origin_country_id", flight.OriginCountryId),
                    new NpgsqlParameter("@_destination_country_id", flight.DestinationCountryId),
                    new NpgsqlParameter("@_departure_time", flight.DepartureTime),
                    new NpgsqlParameter("@_landing_time", flight.LandingTime),
                    new NpgsqlParameter("@_remaining_tickets", flight.RemainingTickets)
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
