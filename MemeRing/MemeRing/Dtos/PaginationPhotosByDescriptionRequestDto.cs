using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model of getting pagination photos by their description from the UI request.
    /// </summary>
    public class PaginationPhotosByDescriptionRequestDto
    {
        /// <summary>
        /// Gets or sets the description of the <see cref="PaginationPhotosByDescriptionRequestDto"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the count of elements on the page of the <see cref="PaginationPhotosByDescriptionRequestDto"/>.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the number of current page of the <see cref="PaginationPhotosByDescriptionRequestDto"/>.
        /// </summary>
        public int PageNumber { get; set; }
    }
}
