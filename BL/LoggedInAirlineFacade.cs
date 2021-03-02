using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using Domain.Entities;
using System.Collections.Generic;

namespace BL
{
    class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            try
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to cancel flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                _flightDAO.Remove(flight);
            }
            catch (NotAllowedAirlineActionException)
            {
                throw;
            }
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
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
            catch (WrongPasswordException)
            {
                throw;
            }
       }

        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            try
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to add flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                _flightDAO.Add(flight);
            }
            catch (NotAllowedAirlineActionException)
            {
                throw;
            }
        }

        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            return _flightDAO.GetFlightsByAirlineCompany(token.User);
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            return _ticketDAO.GetTicketsByAirlineCompany(token.User);
        }

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airlineCompany)
        {
            try
            {
                if (token.User != airlineCompany)
                    throw new NotAllowedAirlineActionException($"{token.User.Name} company not allowed to modify the details of {airlineCompany.Name} company");

                _airlineDAO.Update(airlineCompany);
            }
            catch (NotAllowedAirlineActionException)
            {
                throw;
            }
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            try
            {
                if (token.User != flight.AirlineCompany)
                    throw new NotAllowedAirlineActionException($"Airline company {token.User.Name} not allowed to update flight {flight.Id} that belongs to {flight.AirlineCompany.Name}");

                _flightDAO.Update(flight);
            }
            catch (NotAllowedAirlineActionException)
            {
                throw;
            }
        }
    }
}
