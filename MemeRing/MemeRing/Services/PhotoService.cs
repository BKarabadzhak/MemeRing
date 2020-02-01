using MemeRing.Contexts;
using MemeRing.Dtos;
using MemeRing.Models;
using MemeRing.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MemeRing.Services
{
    /// <summary>
    /// The wrapper for DataService class.
    /// Handles procedures with photos.
    /// </summary>
    public class PhotoService
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;

        public PhotoService(IDataService dataService, IAuthService authService)
        {
            _dataService = dataService;
            _authService = authService;
        }


        /// <summary>
        /// Get specific photo by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> photo id.</param>
        /// <returns>Returns stream of type <see cref="FileStream"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when user hasn`t photo with this id.</exception>
        public FileStream GetPhoto(Guid id)
        {
            try
            {
                var photoFromDb = _dataService.GetPhoto(id);

                if (photoFromDb == null)
                {
                    throw new InvalidOperationException("User hasn`t this photo.");
                }

                var image = System.IO.File.OpenRead($"{photoFromDb.Path}");

                return image;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Get collection of pagination photos of specific user by its id, the number of all pages, the number of all photos, the number of specific page.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> user id.</param>
        /// <param name="paginationPhotosRequestDto">Object of type <see cref="PaginationPhotosRequestDto"/>.</param>
        /// <returns>Returns objects of type <see cref="PaginationPhotosResponseDto"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when user with this id hasn`t photos.</exception>
        public PaginationPhotosResponseDto GetPaginationPhotosByUserId(Guid id, PaginationPhotosRequestDto paginationPhotosRequestDto)
        {
            try
            {
                var allPhotosCountByUserId = _dataService.GetCountAllPhotosByUserId(id);

                int skipNumber = (paginationPhotosRequestDto.PageNumber - 1) * paginationPhotosRequestDto.PageSize;

                int takeNumber = paginationPhotosRequestDto.PageSize;

                var photoFromDb = _dataService.GetPaginationPhotosByUserId(id, skipNumber, takeNumber);

                if (photoFromDb.Count() == 0)
                {
                    throw new InvalidOperationException();
                }

                var allPagesNumber = _dataService.GetAllPagesNumber(allPhotosCountByUserId, paginationPhotosRequestDto.PageSize);

                return new PaginationPhotosResponseDto { AllPagesNumber = allPagesNumber, AllPhotosNumber = allPhotosCountByUserId, Photos = photoFromDb };
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Get collection of pagination photos by their description and the number of all pages.
        /// </summary>
        /// <param name="photoToSearchDto">Object of type <see cref="PaginationPhotosByDescriptionRequestDto"/>.</param>
        /// <returns>Object of type <see cref="PaginationPhotosByDescriptionResponseDto"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when haven`t photos with this description.</exception>
        public PaginationPhotosByDescriptionResponseDto GetPaginationPhotosByDescription(PaginationPhotosByDescriptionRequestDto photoToSearchDto)
        {
            try
            {
                var allPhotosWithDescriptionCount = _dataService.GetCountPhotosByDescription(photoToSearchDto.Description);

                int skipNumber = (photoToSearchDto.PageNumber - 1) * photoToSearchDto.PageSize;

                int takeNumber = photoToSearchDto.PageSize;

                var photosFromDb = _dataService.GetPaginationPhotosByDescription(photoToSearchDto.Description, skipNumber, takeNumber);

                if (photosFromDb.Count() == 0)
                {
                    throw new InvalidOperationException();
                }

                var allPagesNumber = _dataService.GetAllPagesNumber(allPhotosWithDescriptionCount, photoToSearchDto.PageSize);

                return new PaginationPhotosByDescriptionResponseDto { Photo = photosFromDb, AllPagesNumber = allPagesNumber };
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Get collection of pagination photos by description and the number of all pages.
        /// </summary>
        /// <param name="photoToSearchDto">Object of type <see cref="PaginationPhotosByDescriptionRequestDto"/>.</param>
        /// <param name="id"><see cref="Guid"/> user id.</param>
        /// <returns>Objects of type <see cref="PaginationPhotosByDescriptionResponseDto"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when this user has no photos with this description.</exception>
        public PaginationPhotosByDescriptionResponseDto GetPaginationPhotosByDescriptionAndUserId(PaginationPhotosByDescriptionRequestDto photoToSearchDto, Guid id)
        {
            try
            {
                var allPhotosWithDescriptionCount = _dataService.GetCountPhotosByDescriptionAndUserId(photoToSearchDto.Description, id);

                int skipNumber = (photoToSearchDto.PageNumber - 1) * photoToSearchDto.PageSize;

                int takeNumber = photoToSearchDto.PageSize;

                var photosFromDb = _dataService.GetPaginationPhotosByDescriptionAndUserId(photoToSearchDto.Description, skipNumber, takeNumber, id);

                if (photosFromDb.Count() == 0)
                {
                    throw new InvalidOperationException();
                }

                var allPagesNumber = _dataService.GetAllPagesNumber(allPhotosWithDescriptionCount, photoToSearchDto.PageSize);

                return new PaginationPhotosByDescriptionResponseDto { Photo = photosFromDb, AllPagesNumber = allPagesNumber };
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Upload image to the system.
        /// </summary>
        /// <param name="fileToUploadDto">Object of type <see cref="FileToUploadDto"/>.</param>
        /// <param name="userId"><see cref="Guid"/> user id.</param>
        /// <exception cref="InvalidOperationException">Thrown when user with this id isn`t created or cannot save changes in database.</exception>
        public void UploadFile(FileToUploadDto fileToUploadDto, Guid userId)
        {
            try
            {
                var firstIndex = fileToUploadDto.Content.IndexOf(',');
                fileToUploadDto.Content = fileToUploadDto.Content.Substring(firstIndex + 1, fileToUploadDto.Content.Length - firstIndex - 1);

                string directoryPath = $"..\\pictures\\{userId}";

                if (!_authService.UserExists(userId))
                {
                    throw new InvalidOperationException("User with this id isn`t created.");
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string extension = Path.GetExtension(fileToUploadDto.FileName);

                Photo photoToAdd = new Photo();
                photoToAdd.Id = Guid.NewGuid();

                var fileName = photoToAdd.Id;

                photoToAdd.Description = fileToUploadDto.Description;

                photoToAdd.Path = $"{directoryPath}\\{fileName}{extension}";

                photoToAdd.UserId = userId;
                using (FileStream fileStream = new FileStream($"{directoryPath}\\{fileName}{extension}", FileMode.OpenOrCreate))
                {
                    var fileContent = Convert.FromBase64String(fileToUploadDto.Content);
                    fileStream.Write(fileContent);
                    fileStream.Flush();
                }

                _dataService.Add<Photo>(photoToAdd);

                if (!_dataService.SaveAll())
                {
                    throw new InvalidOperationException("Cannot save changes in database.");
                }
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Delete image from the system.
        /// </summary>
        /// <param name="photoId"><see cref="Guid"/> photo id.</param>
        /// <exception cref="InvalidOperationException">Thrown when photo doesn`t exist or cannot save changes in database.</exception>
        public void DeletePhoto(Guid photoId)
        {
            try
            {
                var photoFromDb = _dataService.GetPhoto(photoId);

                if (photoFromDb == null)
                {
                    throw new InvalidOperationException("Photo doesn`t exist.");
                }

                var rootFolder = photoFromDb.Path.Substring(0, 48);

                var lastIndex = photoFromDb.Path.LastIndexOf('\\');
                var authorsFile = photoFromDb.Path.Substring(lastIndex + 1, photoFromDb.Path.Length - lastIndex - 1);

                if (!File.Exists(Path.Combine(rootFolder, authorsFile)))
                {
                    throw new InvalidDataException("File doesn`t exist in this folder.");
                }

                File.Delete(Path.Combine(rootFolder, authorsFile));

                _dataService.Delete(photoFromDb);

                if (!_dataService.SaveAll())
                {
                    throw new InvalidOperationException("Cannot save changes in database.");
                }
            }
            catch (InvalidDataException invalidDataException)
            {
                throw invalidDataException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Get object <see cref="Photo"/> by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> photo id.</param>
        /// <returns>Returns object <see cref="Photo"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when photo doesn`t exist.</exception>
        public Photo GetPhotoObject(Guid id)
        {
            try
            {
                var photoFromDb = _dataService.GetPhoto(id);

                if (photoFromDb == null)
                {
                    throw new InvalidOperationException("Photo doesn`t exist.");
                }

                return photoFromDb;
            }

            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Update specific image in database.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> photo id.</param>
        /// <param name="photoToUpdateDto">Object of type <see cref="PhotoToUpdateDto"/>.</param>
        /// <exception cref="InvalidOperationException">Thrown when photo doesn`t exist or cannot save changes in database.</exception>
        public void UpdatePhoto(Guid id, PhotoToUpdateDto photoToUpdateDto)
        {
            try
            {
                var photoFromDb = _dataService.GetPhoto(id);

                if (photoFromDb == null)
                {
                    throw new InvalidOperationException("Photo doesn`t exist.");
                }

                photoFromDb.Description = photoToUpdateDto.Description;

                _dataService.Update<Photo>(photoFromDb);

                if (!_dataService.SaveAll())
                {
                    throw new InvalidOperationException("Cannot save changes in database.");
                }
            }

            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Get collection of pagination photos of specific user by its id, the number of all pages, the number of all photos, the number of specific page.
        /// </summary>
        /// <param name="paginationGettingDto">Object of type <see cref="PaginationPhotosRequestDto"/>.</param>
        /// <returns>Returns objects of type <see cref="PaginationPhotosResponseDto"/>.</returns>
        /// <exception cref="InvalidDataException">Thrown when photo doesn`t exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when number of photos returned is zero.</exception>
        public PaginationPhotosResponseDto GetPaginationPhotos(PaginationPhotosRequestDto paginationGettingDto)
        {
            try
            {
                var allPhotosNumber = _dataService.GetCountAllPhotos();

                var allPagesNumber = _dataService.GetAllPagesNumber(allPhotosNumber, paginationGettingDto.PageSize);

                int skipNumber = (paginationGettingDto.PageNumber - 1) * paginationGettingDto.PageSize;

                int takeNumber = paginationGettingDto.PageSize;

                var photosFromDb = _dataService.GetPaginationPhotos(skipNumber, takeNumber);

                if (photosFromDb == null)
                {
                    throw new InvalidDataException("Photos don`t exist.");
                }

                if (photosFromDb.Count() == 0)
                {
                    throw new InvalidOperationException();
                }

                var pageNumber = paginationGettingDto.PageNumber;

                return new PaginationPhotosResponseDto { AllPhotosNumber = allPhotosNumber, AllPagesNumber = allPagesNumber, Photos = photosFromDb, PageNumber = pageNumber };
            }

            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
            catch (InvalidDataException invalidDataException)
            {
                throw invalidDataException;
            }
        }
    }
}