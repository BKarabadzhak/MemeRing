using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Models
{
    /// <summary>
    /// Represents a model for user. 
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user id of the <see cref="User"/>.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user name of the <see cref="User"/>.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the <see cref="User"/>.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the collection of photos of the <see cref="User"/>.
        /// </summary>
        public ICollection<Photo> Photos { get; set; }
    }
}
