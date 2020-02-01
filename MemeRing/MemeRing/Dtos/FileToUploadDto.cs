using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Dtos
{
    /// <summary>
    /// Represents a model for a upload photos from the UI request. 
    /// </summary>
    public class FileToUploadDto
    {
        /// <summary>
        /// Gets or sets the file name of the <see cref="FileToUploadDto"/>.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="FileToUploadDto"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the content of the <see cref="FileToUploadDto"/>.
        /// </summary>
        public string Content { get; set; }
    }
}
