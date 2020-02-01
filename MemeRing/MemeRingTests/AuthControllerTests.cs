using System;
using Xunit;
using Moq;
using MemeRing.Services.Interfaces;
using MemeRing.Controllers;
using MemeRing.Models;
using Microsoft.AspNetCore.Mvc;
using MemeRing.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MemeRingTests
{
    public class AuthControllerTests
    {
        [Fact]
        public void Register_NullUser_ThrowsException()
        {
            var authServiceMock = new Mock<IAuthService>();

            var userForRegister = new UserForRegisterDto
            {
                Username = "User123",
                Password = "12345678",
                ConfirmPassword = "12345678"
            };

            authServiceMock.Setup(u => u.Register(userForRegister)).Throws(new Exception("Internal server error."));

            var controller = new AuthController(authServiceMock.Object);

            var result = controller.Register(userForRegister);

            var statusCode = ((ObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((ObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 500);
            Assert.Equal("Internal server error.", dictionary["Error"]);

            authServiceMock.VerifyAll();
        }


        [Fact]
        public void Register_ValidUser_ReturnCreatedUser()
        {
            var user = new User { Username = "Boris", Password = BCrypt.Net.BCrypt.GenerateSalt(12) };
            var userToReturn = new UserResponseDto { Id = Guid.Parse("e4ce0161-4adc-46be-88b8-ea38de2274f0"), Username = "Boris" };

            var authServiceMock = new Mock<IAuthService>();

            var userToRegister = new UserForRegisterDto { Username = "Boris", Password = "password", ConfirmPassword = "password" };

            authServiceMock.Setup(u => u.Register(userToRegister)).Returns(userToReturn);

            var controller = new AuthController(authServiceMock.Object);

            var result = controller.Register(userToRegister);

            var statusCode = ((CreatedAtActionResult)result).StatusCode;

            Assert.True(statusCode == 201);

            authServiceMock.VerifyAll();
        }


        [Fact]
        public void Register_UsernameNotUnique_ThrowsException()
        {
            var authServiceMock = new Mock<IAuthService>();

            var userToRegister = new UserForRegisterDto { Username = "Boris", Password = "123456789", ConfirmPassword = "123456789" };

            authServiceMock.Setup(m => m.Register(userToRegister)).Throws(new InvalidOperationException("Username already exists."));

            var controller = new AuthController(authServiceMock.Object);

            var result = controller.Register(userToRegister);

            var statusCode = ((BadRequestObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((BadRequestObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 400);

            Assert.Equal("Username already exists.", dictionary["Message"]);

            authServiceMock.VerifyAll();
        }


        [Fact]
        public void Login_NullUser_ThrowsException()
        {
            var authServiceMock = new Mock<IAuthService>();

            var userToLogin = new UserForLoginDto { Username = "Boris", Password = "password" };

            authServiceMock.Setup(u => u.Login(userToLogin.Username, userToLogin.Password)).Throws(new InvalidOperationException("User is not registered in system."));

            var controller = new AuthController(authServiceMock.Object);

            var result = controller.Login(userToLogin);

            var statusCode = ((ObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((ObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 400);
            Assert.Equal("User is not registered in system.", dictionary["Message"]);

            authServiceMock.VerifyAll();
        }


        [Fact]
        public void Login_UnregisteredUser_ThrowsException()
        {
            var authServiceMock = new Mock<IAuthService>();

            var controller = new AuthController(authServiceMock.Object);

            var result = controller.Login(null);

            var statusCode = ((ObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((ObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 500);
            Assert.Equal("Object reference not set to an instance of an object.", dictionary["Error"]);
        }


        [Fact]
        public void Login_ValidUser_ReturnToken()
        {
            var authServiceMock = new Mock<IAuthService>();

            Guid userId = Guid.NewGuid();

            var userToLogin = new UserForLoginDto { Username = "Boris", Password = "password" };

            var user = new UserResponseDto() { Username = userToLogin.Username, Id = userId };

            var tokenToReturn = "D4569KRTeKj_kqdAVrAiPbpRloAfE1fqp0eVAJ-IChQcV-kv3gW-gBAzWztBEdGGY";

            var loginResponseDto = new LoginResponseDto() { token = tokenToReturn, user = user };

            authServiceMock.Setup(u => u.Login(userToLogin.Username, userToLogin.Password)).Returns(loginResponseDto);

            var controller = new AuthController(authServiceMock.Object);

            var result = controller.Login(userToLogin);

            var statusCode = ((OkObjectResult)result).StatusCode;

            var jsonValue = JsonConvert.SerializeObject(((OkObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 200);
            Assert.Equal("D4569KRTeKj_kqdAVrAiPbpRloAfE1fqp0eVAJ-IChQcV-kv3gW-gBAzWztBEdGGY", dictionary["token"]);

            authServiceMock.VerifyAll();
        }

    }
}
