using BL;
using BL.LoginService;
using FlightsManagmentSystemWebAPI.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightsManagmentSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        FlightCenterSystem system = FlightCenterSystem.GetInstance();
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IJwtAuthManager jwtAuthManager, ILogger<AccountController> logger)
        {
            _jwtAuthManager = jwtAuthManager;
            _logger = logger;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)//maybe it's not neccessary
                return BadRequest();

            if (!system.TryLogin(request.UserName, request.Password, out ILoginToken loginToken, out FacadeBase facade))//validate the login credentials
                return Unauthorized();

            dynamic user = loginToken.GetType().GetProperties()[0].GetValue(loginToken);//Get the user from the login token as dynamic object

            string role = user.User.UserRole.ToString();//get the user role

            var claims = new[]//generate claims array
            {
                new Claim(ClaimTypes.Name, user.User.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.User.Id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("LoginToken", loginToken.ToString()),
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);//Invoke GenerateTokens and get JWTAuthResult
            _logger.LogInformation($"User [{request.UserName}] logged in the system.");
            return Ok(new LoginResult
            {
                UserName = request.UserName,
                Role = role,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()//The logout method invalidates the refresh token on the server-side,
        {                           //In order to invalidate the JWT access token on the server-side block-list strategy can be used or just keep the exp of the token short 
            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);//remove the refresh token from the dictionary
            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))//check if the request token is not provided
                    return Unauthorized();
                

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");//Get the Jwt access token
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);//Refresh the token and get JWTAuthResult
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return Ok(new LoginResult
                {
                    UserName = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }
    }

    /// <summary>
    /// Login request model
    /// </summary>
    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    /// <summary>
    /// Login result model
    /// </summary>
    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    /// <summary>
    /// Refresh token request model
    /// </summary>
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

}
