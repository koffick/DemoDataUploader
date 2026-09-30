using AutoFixture;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.Api.Models;
using WebApi.Host.Controllers;

namespace WebApi.Tests.AuthcontrollerTests
{
    public class AuthControllersTests
    {
        private AuthController _controller;
        private Mock<IUserRepository> _userRepository;

        public AuthControllersTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _controller = new AuthController(_userRepository.Object);
        }

        [Fact]
        public void Login_ShouldReturnOkCorrect()
        {
            var login = Guid.NewGuid().ToString();
            var userInfo = new Fixture().Create<UserInfo>();
            _userRepository.Setup(s => s.Find(login)).Returns(userInfo);

            var result = _controller.Login(login);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var token = (result as OkObjectResult).Value.ToString();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var name = jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Name);
            Assert.NotNull(name);
            Assert.Equal(AuthOptions.ISSUER, jwtSecurityToken.Issuer);
            Assert.Equal(AuthOptions.AUDIENCE, jwtSecurityToken.Audiences.FirstOrDefault());
        }
    }
}
