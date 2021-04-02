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
        }

        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)//maybe it's not the best sulotion to delete. what to do with the tickets?????
        {
            Execute(() =>
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to cancel flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                _flightDAO.Remove(flight);
            }, new { Token = token, Flight = flight }, _logger);
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            Execute(() =>
            {
                if (token.User.User.Password != oldPassword)
                    throw new WrongPasswordException($"User {token.User.User.UserName} tried to update password with wrong old password");

                if (oldPassword == newPassword)
                    throw new WrongPasswordException($"User {token.User.User.UserName} haven't been able to update the password- entered same new password");

                User user = token.User.User;
                user.Password = newPassword;

                _userDAO.Update(user);
            }, new { Token = token, OldPassword = oldPassword, NewPassword = newPassword }, _logger);
        }

        public long CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            long result = 0;

            result = Execute(() =>
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to add flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                result = _flightDAO.Add(flight);
                return result;
            }, new { Token = token, Flight = flight }, _logger);

            return result;
        }

        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            IList<Flight> result = null;

            result = Execute(() => _flightDAO.GetFlightsByAirlineCompany(token.User), new { Token = token }, _logger);

            return result;
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            IList<Ticket> result = null;

            result = Execute(() => _ticketDAO.GetTicketsByAirlineCompany(token.User), new { Token = token }, _logger);

            return result;
        }

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airlineCompany)
        {
            Execute(() =>
            {
                if (token.User != airlineCompany)
                    throw new NotAllowedAirlineActionException($"{token.User.Name} company not allowed to modify the details of {airlineCompany.Name} company");

                _airlineDAO.Update(airlineCompany);
            }, new { Token = token, AirlineCompany = airlineCompany }, _logger);
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            Execute(() =>
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to update flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                _flightDAO.Update(flight);
            }, new { Token = token, Flight = flight }, _logger);
        }
    }
}
