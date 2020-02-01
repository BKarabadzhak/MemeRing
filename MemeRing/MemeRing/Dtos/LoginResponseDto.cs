using MemeRing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model of response for UI after login user.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Gets or sets the token of the <see cref="FileToUploadDto"/>.
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// Gets or sets the user of the <see cref="FileToUploadDto"/>.
        /// </summary>
        public UserResponseDto user { get; set; }
    }
}
