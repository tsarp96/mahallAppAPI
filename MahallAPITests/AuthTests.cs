using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Castle.Core.Configuration;
using FluentAssertions;
using Flurl.Http.Testing;
using mahallAppAPI;
using mahallAppAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace MahallAPITests
{
    public class UnitTest1
    {

        // TODO: Kullanıcı adı dolu mu ? + 
        // TODO: Şıfre dolu mu +
        // TODO: Sıfreyı hashle +
        // TODO: Kullanıcı var mı? yoksa hata +
        // TODO: Kullanıcı bılgılırını don + 

        private Mock<IHashHelper> _hashHelper = new Mock<IHashHelper>(MockBehavior.Strict);
        private Mock<IUserRepository> _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        private Mock<IAuthenticationService> _authenticationService = new Mock<IAuthenticationService>(MockBehavior.Loose);
        private Mock<IConfiguration> _config = new Mock<IConfiguration>(MockBehavior.Loose);
        private Mock<FakeHttpMessageHandler> _fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>(MockBehavior.Loose);

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Auth_should_return_bad_request_when_username_is_null_or_empty(String username)
        {
            // arrange
            var authController = new AuthController(_hashHelper.Object, _userRepository.Object, _authenticationService.Object);
            var authRequest = new AuthRequest();
            authRequest.UserName = username;
            
            // act
            IActionResult result = authController.Auth(authRequest);

            // assert
            ((StatusCodeResult) result).StatusCode.Should().Be(HttpStatusCode.BadRequest.GetHashCode());
            _hashHelper.Verify(x => x.Hash(authRequest.Password), Times.Never);

        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Auth_should_return_bad_request_when_password_is_null_or_empty(String password)
        {
            // arrange
            var authController = new AuthController(_hashHelper.Object, _userRepository.Object, _authenticationService.Object);
            var authRequest = new AuthRequest();
            authRequest.UserName = "asdsad";
            authRequest.Password = password;

            // act
            IActionResult result = authController.Auth(authRequest);

            // assert
            ((StatusCodeResult) result).StatusCode.Should().Be(HttpStatusCode.BadRequest.GetHashCode());

        }
        
        [Fact]
        public void Auth_should_hash_password()
        {
            // arrange
            var authController = new AuthController(_hashHelper.Object, _userRepository.Object, _authenticationService.Object);
            var authRequest = new AuthRequest();
            authRequest.UserName = "asdsad";
            authRequest.Password = "asddfgdf";

            _hashHelper.Setup(x => x.Hash(authRequest.Password)).Returns(Guid.NewGuid().ToString());
            _userRepository.Setup(x => x.GetUserInfo(It.IsAny<string>(), It.IsAny<string>())).Returns((UserInfo) null);

            // act
            IActionResult result = authController.Auth(authRequest);

            // assert
            _hashHelper.Verify(x => x.Hash(authRequest.Password), Times.Once);
        }
        
        [Fact]
        public void Auth_should_return_unauhtorized_when_not_existed_user()
        {
            // arrange
            var authController = new AuthController(_hashHelper.Object, _userRepository.Object, _authenticationService.Object);
            var authRequest = new AuthRequest();
            authRequest.UserName = "asdsad";
            authRequest.Password = "asddfgdf";

            var hashedPassword = Guid.NewGuid().ToString();
            
            _hashHelper.Setup(x => x.Hash(authRequest.Password)).Returns(hashedPassword);
            _userRepository.Setup(x => x.GetUserInfo(authRequest.UserName, hashedPassword)).Returns((UserInfo) null);
            
            // act
            IActionResult result = authController.Auth(authRequest);

            // assert
            ((StatusCodeResult) result).StatusCode.Should().Be(HttpStatusCode.Unauthorized.GetHashCode());
            _userRepository.Verify(x => x.GetUserInfo(authRequest.UserName, hashedPassword), Times.Once);


        }
        
        [Fact]
        public void Auth_should_return_userinfo_when_user_exist()
        {
            // arrange
            var authController = new AuthController(_hashHelper.Object, _userRepository.Object, _authenticationService.Object);
            var authRequest = new AuthRequest();
            authRequest.UserName = "asdsad";
            authRequest.Password = "asddfgdf";

            var hashedPassword = Guid.NewGuid().ToString();
            
            _hashHelper.Setup(x => x.Hash(authRequest.Password)).Returns(hashedPassword);
            _userRepository.Setup(x => x.GetUserInfo(authRequest.UserName, hashedPassword)).Returns(new UserInfo());
            
            // act
            IActionResult result = authController.Auth(authRequest);
            

            // assert
            ((OkObjectResult) result).StatusCode.Should().Be(HttpStatusCode.OK.GetHashCode());
            _userRepository.Verify(x => x.GetUserInfo(authRequest.UserName, hashedPassword), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Auth_should_return_unauhtorized_when_token_is_invalid()
        {
            // ARRANGE
                // AuthController ındaki Auth methodunu gerekli kullanıcı adına çalıştır

            // ACT


            // ASERT

        }
    }
}
