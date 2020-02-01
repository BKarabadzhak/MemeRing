using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model of getting pagination photos for UI request.
    /// </summary>
    public class PaginationPhotosRequestDto
    {
        /// <summary>
        /// Gets or sets the number of current page of the <see cref="PaginationPhotosRequestDto"/>.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the count of elements on the page of the <see cref="PaginationPhotosRequestDto"/>.
        /// </summary>
        public int PageSize { get; set; }
    }
}
