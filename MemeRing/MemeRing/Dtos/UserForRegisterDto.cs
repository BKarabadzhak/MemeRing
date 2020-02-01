using System;
using System.ComponentModel.DataAnnotations;

namespace MemeRing.Services.Interfaces
{
    /// <summary>
    /// Represents a model for register user from the UI request. 
    /// </summary>
    public class UserForRegisterDto
    {
        /// <summary>
        /// Gets or sets the user name of the <see cref="UserForRegisterDto"/>.
        /// </summary>
        [Required(ErrorMessage = "Not specified Username")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "You must specify a username between 6 and 50 characters.")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the <see cref="UserForRegisterDto"/>.
        /// </summary>
        [Required(ErrorMessage = "Not specified Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "You must specify a password between 6 and 50 characters.")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password of the <see cref="UserForRegisterDto"/>.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
