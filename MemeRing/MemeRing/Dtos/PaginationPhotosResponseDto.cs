using MemeRing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model of getting pagination photos for UI response.
    /// </summary>
    public class PaginationPhotosResponseDto
    {
        /// <summary>
        /// Gets or sets the number of all pages of the <see cref="PaginationPhotosResponseDto"/>.
        /// </summary>
        public int AllPagesNumber { get; set; }

        /// <summary>
        /// Gets or sets the collection of pagination photos of the <see cref="PaginationPhotosResponseDto"/>.
        /// </summary>
        public IEnumerable<Photo> Photos { get; set; }

        /// <summary>
        /// Gets or sets the number of all photos of the <see cref="PaginationPhotosResponseDto"/>.
        /// </summary>
        public int AllPhotosNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of specific page of the <see cref="PaginationPhotosResponseDto"/>.
        /// </summary>
        public int PageNumber { get; set; }

    }
}
