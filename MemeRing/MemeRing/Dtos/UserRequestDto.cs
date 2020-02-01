using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model for user from the UI request. 
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        /// Gets or sets the user id of the <see cref="UserResponseDto"/>.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user name of the <see cref="UserResponseDto"/>.
        /// </summary>
        public string Username { get; set; }
         
    }
}
