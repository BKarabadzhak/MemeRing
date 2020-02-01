using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model for a update photos from the UI request. 
    /// </summary>
    public class PhotoToUpdateDto
    {
        /// <summary>
        /// Gets or sets the description of the <see cref="PhotoToUpdateDto"/>.
        /// </summary>
        public string Description { get; set; }
    }
}
