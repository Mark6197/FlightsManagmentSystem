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
        public override void Add(Flight flight)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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

                var id = command.ExecuteScalar();
            }
        }

        public override Flight Get(int id)
        {
            Flight flight = new Flight();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flight;
        }

        public override IList<Flight> GetAll()
        {
            List<Flight> flights = new List<Flight>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights;
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            Dictionary<Flight,int> flights_vacancy_dict = new Dictionary<Flight,int>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights_vacancy_dict;
        }

        public IList<Flight> GetFlightsByAirlineCompany(AirlineCompany airlineCompany)
        {
            List<Flight> flights = new List<Flight>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights;
        }

        public IList<Flight> GetFlightsByCustomer(Customer customer)
        {
            List<Flight> flights = new List<Flight>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights;
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            List<Flight> flights = new List<Flight>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights;
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryId)
        {
            List<Flight> flights = new List<Flight>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights;
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            List<Flight> flights = new List<Flight>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights;
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryId)
        {
            List<Flight> flights = new List<Flight>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return flights;
        }

        public override void Remove(Flight flight)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_remove_flight(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", flight.Id));
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }

        public override void Update(Flight flight)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }
    }
}
