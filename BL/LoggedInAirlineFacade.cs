using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using DAL;
using Domain.Entities;
using System.Collections.Generic;
using System.Reflection;

namespace BL
{
    public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public LoggedInAirlineFacade() : base()
        {
            _userDAO = new UserDAOPGSQL();
            _airlineDAO = new AirlineDAOPGSQL();
            _ticketDAO = new TicketDAOPGSQL();
        }

        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)//maybe it's not the best sulotion to delete. what to do with the tickets?????
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({flight})");

            try
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to cancel flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                _flightDAO.Remove(flight);
            }

            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({oldPassword},{newPassword})");

            try
            {
                if (token.User.User.Password != oldPassword)
                    throw new WrongPasswordException($"User {token.User.User.UserName} tried to update password with wrong old password");

                if (oldPassword == newPassword)
                    throw new WrongPasswordException($"User {token.User.User.UserName} haven't been able to update the password- entered same new password");

                User user = token.User.User;
                user.Password = newPassword;

                _userDAO.Update(user);
            }
            //catch (WrongPasswordException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public long CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({flight})");
            long flight_id = 0;
            try
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to add flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                flight_id = _flightDAO.Add(flight);
                return flight_id;
            }
            //catch (NotAllowedAirlineActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {flight_id}");
            }
        }

        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _flightDAO.GetFlightsByAirlineCompany(token.User);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _ticketDAO.GetTicketsByAirlineCompany(token.User);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airlineCompany)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({airlineCompany})");

            try
            {
                if (token.User != airlineCompany)
                    throw new NotAllowedAirlineActionException($"{token.User.Name} company not allowed to modify the details of {airlineCompany.Name} company");

                _airlineDAO.Update(airlineCompany);
            }
            //catch (NotAllowedAirlineActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({flight})");

            try
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to update flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                _flightDAO.Update(flight);
            }
            //catch (NotAllowedAirlineActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }
    }
}
