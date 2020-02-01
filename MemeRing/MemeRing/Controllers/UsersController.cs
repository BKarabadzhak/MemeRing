using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MemeRing.Dtos;
using MemeRing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemeRing.Controllers
{
    /// <summary>
    /// This controller is used to get user or users.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        private string _userName => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        public UsersController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get collection of all users from system.
        /// </summary>
        /// <returns>200Ok response.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _dataService.GetUsers();

                var usersToReturn = _mapper.Map<IEnumerable<UserResponseDto>>(users);

                return Ok(usersToReturn);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return NoContent();
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
        /// Get specific user by userId.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>200Ok response.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("{id}", Name = "GetUser")]
        public IActionResult GetUser(Guid id)
        {
            try
            {
                var user = _dataService.GetUser(id);

                var userToReturn = _mapper.Map<UserResponseDto>(user);

                return Ok(userToReturn);
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
