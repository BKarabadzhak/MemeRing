using MemeRing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model of getting pagination photos by their description for UI response.
    /// </summary>
    public class PaginationPhotosByDescriptionResponseDto
    {

        /// <summary>
        /// Gets or sets the collection of pagination photos of the <see cref="PaginationPhotosByDescriptionResponseDto"/>.
        /// </summary>
        public IEnumerable<Photo> Photo { get; set; }

        /// <summary>
        /// Gets or sets the number of all pages of the <see cref="PaginationPhotosByDescriptionResponseDto"/>.
        /// </summary>
        public int AllPagesNumber { get; set; }
    }
}
