using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MemeRing.Dtos;
using MemeRing.Models;
using MemeRing.Services;
using MemeRing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http.Headers;

namespace MemeRing.Controllers
{
    /// <summary>
    /// This controller is used to manipulate photos.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly PhotoService _photoService;

        public PhotosController(IDataService dataService, IAuthService authService)
        {
            _photoService = new PhotoService(dataService, authService);
        }


        /// <summary>
        /// Get photo by its <see cref="Guid"/>.
        /// </summary>
        /// <param name="id">Id required photo.</param>
        /// <returns>200Ok.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("{id}", Name = "GetPhoto")]
        public IActionResult GetPhoto(Guid id)
        {
            try
            {
                var image = _photoService.GetPhoto(id);

                return File(image, "image/jpeg");
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Get pagination photos by their descriptions, count of elements on the page (PageSize) and number of current page (PageNumber), which are contained in the class <see cref="PaginationPhotosByDescriptionRequestDto"/>.
        /// </summary>
        /// <param name="paginationPhotosByDescriptionRequestDto">Info about description, count of elements on the page and number of current page.</param>
        /// <returns>200Ok.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Description")]
        public IActionResult GetPaginationPhotosByDescription(PaginationPhotosByDescriptionRequestDto paginationPhotosByDescriptionRequestDto)
        {
            try
            {
                var photoToSearch = _photoService.GetPaginationPhotosByDescription(paginationPhotosByDescriptionRequestDto);

                return Ok(photoToSearch);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return NoContent();
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Get pagination photos by their descriptions, count of elements on the page (PageSize), number of current page (PageNumber) and current user id, which are contained in the class <see cref="PaginationPhotosByDescriptionRequestDto"/>.
        /// </summary>
        /// <param name="paginationPhotosByDescriptionRequestDto">Info about description, count of elements on the page and number of current page.</param>
        /// <returns>200Ok.</returns>
        [HttpPost]
        [Route("UserDescription")]
        public IActionResult GetPaginationPhotosOfCurrentUserByDescription(PaginationPhotosByDescriptionRequestDto paginationPhotosByDescriptionRequestDto)
        {
            try
            {
                var photoToSearch = _photoService.GetPaginationPhotosByDescriptionAndUserId(paginationPhotosByDescriptionRequestDto, Guid.Parse(User.Identity.Name));

                return Ok(photoToSearch);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return NoContent();
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }



        /// <summary>
        /// Get pagination photos of random user by descriptions of photo, count of elements on the page (PageSize), number of current page (PageNumber) and current user id, which are contained in the class <see cref="PaginationPhotosByDescriptionRequestDto"/>.
        /// </summary>
        /// <param name="id">User id of type <see cref="Guid"/>.</param>
        /// <param name="paginationPhotosByDescriptionRequestDto">Info about description, count of elements on the page and number of current page.</param>
        /// <returns>200Ok.</returns>
        [HttpPost]
        [Route("UserDescription/{id}")]
        public IActionResult GetPaginationPhotosByUserIdAndDescription(Guid id, PaginationPhotosByDescriptionRequestDto paginationPhotosByDescriptionRequestDto)
        {
            try
            {
                var photoToSearch = _photoService.GetPaginationPhotosByDescriptionAndUserId(paginationPhotosByDescriptionRequestDto, id);

                return Ok(photoToSearch);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return NoContent();
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }



        /// <summary>
        /// Get pagination photos by their count of elements on the page (PageSize) and number of current page (PageNumber), which are contained in the class <see cref="PaginationPhotosRequestDto"/>.
        /// </summary>
        /// <param name="paginationPhotosRequestDto">Info about count of elements on the page and number of current page.</param>
        /// <returns>200Ok.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Pagination")]
        public IActionResult GetPaginationPhotos(PaginationPhotosRequestDto paginationPhotosRequestDto)
        {
            try
            {
                var pagination = _photoService.GetPaginationPhotos(paginationPhotosRequestDto);

                return Ok(new { pagination });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return NoContent();
            }
            catch (InvalidDataException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Get pagination photos of current user by their count of elements on the page (PageSize) and number of current page (PageNumber), which are contained in the class <see cref="PaginationPhotosRequestDto"/>.
        /// </summary>
        /// <param name="paginationDto">Info about count of elements on the page and number of current page.</param>
        /// <returns>200Ok.</returns>
        [HttpPost]
        [Route("UserPhotos")]
        public IActionResult GetAllUserPaginationPhotos(PaginationPhotosRequestDto paginationDto)
        {
            try
            {
                var pagination = _photoService.GetPaginationPhotosByUserId(Guid.Parse(User.Identity.Name), paginationDto);

                return Ok(new { pagination });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return NoContent();
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Get pagination photos of specific user by their count of elements on the page (PageSize) and number of current page (PageNumber), which are contained in the class <see cref="PaginationPhotosRequestDto"/>.
        /// </summary>
        /// <param name="id">.</param>
        /// <param name="paginationDto">Info about count of elements on the page and number of current page.</param>
        /// <returns>200Ok.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("UserPhotos/{id}")]
        public IActionResult GetPaginationPhotosByUserId(Guid id, PaginationPhotosRequestDto paginationDto)
        {
            try
            {
                var pagination = _photoService.GetPaginationPhotosByUserId(id, paginationDto);

                return Ok(new { pagination });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return NoContent();
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Upload image in system. <see cref="FileToUploadDto"/> contains name of file, description of file and content in base64.
        /// </summary>
        /// <param name="fileToUploadDto">Info about name of file, description of file and content in base64.</param>
        /// <returns>200Ok.</returns>
        [HttpPost]
        public IActionResult UploadFile(FileToUploadDto fileToUploadDto)
        {
            try
            {
                _photoService.UploadFile(fileToUploadDto, Guid.Parse(User.Identity.Name));

                var responseString = "Photo was added successfully.";

                return Ok(new { responseString });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Delete image from system by its id.
        /// </summary>
        /// <param name="id">Image id.</param>
        /// <returns>200Ok.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeletePhoto")]
        public IActionResult DeletePhoto(FileToDeleteDto fileToDeleteDto)
        {
            try
            {
                _photoService.DeletePhoto(fileToDeleteDto.PhotoId);

                var responseString = "Photo was deleted successfully.";

                return Ok(new { responseString });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Get image as an object <see cref="Photo"/> from system by its id.
        /// </summary>
        /// <param name="id">Image id.</param>
        /// <returns>200Ok.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("{id}/object")]
        public IActionResult GetPhotoObject(Guid id)
        {
            try
            {
                var photoObject = _photoService.GetPhotoObject(id);

                return Ok(photoObject);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        /// <summary>
        /// Update image by its id and <see cref="PhotoToUpdateDto"/> object.
        /// </summary>
        /// <param name="id">Image id.</param>
        /// <param name="photoToUpdateDto">Object of type <see cref="PhotoToUpdateDto"/> with new description.</param>
        /// <returns>200Ok.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdatePhoto")]
        public IActionResult UpdatePhoto(Guid id, PhotoToUpdateDto photoToUpdateDto)
        {
            try
            {
                _photoService.UpdatePhoto(id, photoToUpdateDto);

                var responseString = "Photo was updated successfully.";

                return Ok(new { responseString });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new { invalidOperationException.Message });
            }
            catch (Exception exeption)
            {
                return new ObjectResult(new
                {
                    Error = exeption.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
