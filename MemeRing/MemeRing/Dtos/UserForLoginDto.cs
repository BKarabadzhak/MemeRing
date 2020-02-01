//using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace MemeRing.Services.Interfaces
{
    /// <summary>
    /// Represents a model for login user from the UI request. 
    /// </summary>
    public class UserForLoginDto
    {
        /// <summary>
        /// Gets or sets the user name of the <see cref="UserForLoginDto"/>.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "You must specify a password between 6 and 50 characters.")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the <see cref="UserForLoginDto"/>.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "You must specify a password between 6 and 50 characters.")]
        public string Password { get; set; }
    }
}
