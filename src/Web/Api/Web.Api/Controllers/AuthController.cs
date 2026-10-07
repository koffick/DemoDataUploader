using DataUploader.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.Api.DTO.Responses;
using Web.Api.Models;

namespace WebApi.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository, ILogger<AuthController> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ApiFailureResponse), 400)]
        public IActionResult Login(string login)
        {
            try
            {
                _logger.LogInformation($"Вход в систему под именем '{login}'.");
                var userInfo = _userRepository.Find(login);
                if (userInfo == null)
                {
                    var message = $"Пользователь с именем '{login}' в сиситеме не зарегистрирован.";
                    _logger.LogInformation(message);
                    return BadRequest(new ApiFailureResponse(message));
                }

                var claims = new List<Claim>() { new Claim(ClaimTypes.Name, userInfo.FullName) };
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                _logger.LogInformation($"Пользователь под именем '{login}' успешно идетифицировался.");
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            catch (Exception ex)
            {
                var message = $"В процессе идентификации под именем '{login}'', произошла непредвиденная ошибка: {ex.Message}";
                _logger.LogError(message, ex);
                return BadRequest(new ApiFailureResponse(message));
            }
        }
    }
}
