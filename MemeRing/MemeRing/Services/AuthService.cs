using AutoMapper;
using MemeRing.Contexts;
using MemeRing.Dtos;
using MemeRing.Models;
using MemeRing.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MemeRing.Services
{
    /// <summary>
    /// The Authorization class.
    /// Handles authorization procedures.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public AuthService(DataContext context, IConfiguration configuration, IDataService dataService, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _dataService = dataService;
            _mapper = mapper;
        }

        /// <summary>
        /// Login user with <see cref="UserForLoginDto.Username"/> and <see cref="UserForLoginDto.Password"/> in system.
        /// </summary>
        /// <param name="username">Used to check username</param>
        /// <param name="password">Used to check password</param>
        /// <returns>Returns <see cref="LoginResponseDto"/> object.</returns>
        /// <exception cref="InvalidOperationException">Thrown when username not registered in system or user entered an invalid password .</exception>
        public LoginResponseDto Login(string username, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => string.Equals(x.Username, username));

                if (user == null)
                {
                    throw new InvalidOperationException("User is not registered in system.");
                }

                if (!VerifyPassword(password, user.Password))
                {
                    throw new InvalidOperationException("User entered an invalid password.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Secret").Value);

                ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.UtcNow.AddHours(9),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var userToResponse = _mapper.Map<UserResponseDto>(user);

                return new LoginResponseDto { token = tokenString, user = userToResponse };
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Register <see cref="UserForRegisterDto"/> in system.
        /// </summary>
        /// <param name="userForRegisterDto">User to be registered.</param>
        /// <returns>Returns an object <see cref="UserResponseDto"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when username already exits in system or cannot save changes in database.</exception>
        /// <exception cref="Exception">Thrown in other cases.</exception>
        public UserResponseDto Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                if (UserExists(userForRegisterDto.Username))
                {
                    throw new InvalidOperationException("Username already exists.");
                }

                var userToCreate = _mapper.Map<User>(userForRegisterDto);

                string passwordHash = CreatePasswordHash(userToCreate.Password);

                userToCreate.Password = passwordHash;
                userToCreate.Id = Guid.NewGuid();

                _dataService.Add<User>(userToCreate);

                if (!_dataService.SaveAll())
                {
                    throw new InvalidOperationException("Cannot save changes in database.");
                }

                var userToReturn = _mapper.Map<UserResponseDto>(userToCreate);

                return userToReturn;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Verifies user password. 
        /// </summary>
        /// <param name="password">User-entered password.</param>
        /// <param name="hashedPassword">User password from the database.</param>
        /// <returns>If passwords match returns true.</returns>
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        /// <summary>
        /// Creates a hashed password. 
        /// </summary>
        /// <param name="password">User-entered password.</param>
        /// <returns>Returns hashed password.</returns>
        private string CreatePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Checks user existence with specific username.
        /// </summary>
        /// <param name="username">User-entered username.</param>
        /// <returns>If user with this name exists in system returns true.</returns>
        public bool UserExists(string username)
        {
            return _context.Users.Any(user => string.Equals(user.Username, username)) ?
                true : false;
        }

        /// <summary>
        /// Checks user existence with specific username.
        /// </summary>
        /// <param name="userId">Id of user.</param>
        /// <returns>If user with this id exists in system returns true.</returns>
        public bool UserExists(Guid userId)
        {
            return _context.Users.Any(user => user.Id == userId) ?
                true : false;
        }
    }
}
