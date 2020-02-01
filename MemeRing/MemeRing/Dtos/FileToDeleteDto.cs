using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    public class FileToDeleteDto
    {
        /// <summary>
        /// Gets or sets the photo id of the <see cref="FileToDeleteDto"/>.
        /// </summary>
        public Guid PhotoId { get; set; }
    }
}