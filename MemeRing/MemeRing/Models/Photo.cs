using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Models
{
    /// <summary>
    /// Represents a model for photo. 
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// Gets or sets the photo id of the <see cref="Photo"/>.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the description of photo of the <see cref="Photo"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the path of photo of the <see cref="Photo"/>.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the user id owner of the <see cref="Photo"/>.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
