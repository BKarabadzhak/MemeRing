using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MemeRing.Dtos;
using MemeRing.Models;
using MemeRing.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemeRing.Controllers
{
    /// <summary>
    /// This controller is used to authorize and authenticate an user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// Register user <see cref="UserForRegisterDto"/> in system.
        /// </summary>
        /// <param name="userForRegisterDto">User to be registered.</param>
        /// <returns>201Created response.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                var createdUser = _authService.Register(userForRegisterDto);

                return CreatedAtAction("GetUser", new { controller = "Users", id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Login user <see cref="UserForLoginDto"/> in system.
        /// </summary>
        /// <param name="userForLoginDto">Used to check user.</param>
        /// <returns>200Ok response.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var loginResponseDto = _authService.Login(userForLoginDto.Username, userForLoginDto.Password);

                return Ok(new { loginResponseDto.token, loginResponseDto.user });

            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
