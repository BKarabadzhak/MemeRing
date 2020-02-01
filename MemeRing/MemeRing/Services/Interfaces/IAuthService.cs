using MemeRing.Dtos;
using MemeRing.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Register <see cref="UserForRegisterDto"/> in system.
        /// </summary>
        /// <param name="userForRegisterDto">User to be registered.</param>
        /// <returns>Returns model of registered user <see cref="UserResponseDto"/>.</returns>
        UserResponseDto Register(UserForRegisterDto userForRegisterDto);

        /// <summary>
        /// Log in user with <see cref="UserForLoginDto.Username"/> and <see cref="UserForLoginDto.Password"/> in system.
        /// </summary>
        /// <param name="username">Used to check username.</param>
        /// <param name="password">Used to check password.</param>
        /// <returns>Returns model of logined user <see cref="LoginResponseDto"/>.</returns>
        LoginResponseDto Login(string username, string password);

        /// <summary>
        /// Checks user existence with specific username.
        /// </summary>
        /// <param name="username"></param>
        bool UserExists(string username);

        /// <summary>
        /// Checks user existence with specific identifier <see cref="Guid"/>.
        /// </summary>
        /// <param name="userId"></param>
        bool UserExists(Guid userId);
    }
}
