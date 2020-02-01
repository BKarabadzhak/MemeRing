using MemeRing.Models;
using System;
using System.Collections.Generic;

namespace MemeRing.Services.Interfaces
{
    public interface IDataService
    {
        /// <summary>
        /// Add specific entity to database.
        /// </summary>
        /// <param name="entity">Entity to be added.</param>
        void Add<T>(T entity);

        /// <summary>
        /// Delete specific entity from database.
        /// </summary>
        /// <param name="entity">Entity to be deleted.</param>
        void Delete<T>(T entity);

        /// <summary>
        /// Update specific entity in database.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        void Update<T>(T entity);

        /// <summary>
        /// Save all changes in database.
        /// </summary>
        bool SaveAll();

        /// <summary>
        /// Get collection of all users from the system.
        /// </summary>
        /// <returns>Returns collection of objects <see cref="User"/>.</returns>
        IEnumerable<User> GetUsers();

        /// <summary>
        /// Get specific user by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> user id.</param>
        /// <returns>Returns object <see cref="User"/>.</returns>
        User GetUser(Guid id);

        /// <summary>
        /// Get specific photo by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> photo id.</param>
        /// <returns>Returns object <see cref="Photo"/>.</returns>
        Photo GetPhoto(Guid id);

        /// <summary>
        /// Get collection of pagination photos of specific user by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> photo id.</param>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        IEnumerable<Photo> GetPaginationPhotosByUserId(Guid id, int skip, int take);

        /// <summary>
        /// Get collection of pagination photos by their description.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        IEnumerable<Photo> GetPaginationPhotosByDescription(string description, int skip, int take);

        /// <summary>
        /// Get count of all photos in system.
        /// </summary>
        int GetCountAllPhotos();

        /// <summary>
        /// Get collection of pagination photos.
        /// </summary>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        IEnumerable<Photo> GetPaginationPhotos(int skip, int take);

        /// <summary>
        /// Get all pages number.
        /// </summary>
        /// <param name="allPhotosNumber">Number of all photos.</param>
        /// <param name="pageSize">Count of photos on page.</param>
        int GetAllPagesNumber(int allPhotosNumber, int pageSize);

        /// <summary>
        /// Get count of photos by their description.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        int GetCountPhotosByDescription(string description);

        /// <summary>
        /// Get count of photos by their description and user id.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        /// <param name="userId"><see cref="Guid"/> user id.</param>
        int GetCountPhotosByDescriptionAndUserId(string description, Guid userId);

        /// <summary>
        /// Get count of photos by <see cref="Guid"/> user id.
        /// </summary>
        /// <param name="userId"><see cref="Guid"/> user id.</param>
        int GetCountAllPhotosByUserId(Guid userId);

        /// <summary>
        /// Get collection of pagination photos by description and <see cref="Guid"/> user id.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <param name="userId"><see cref="Guid"/> user id.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        IEnumerable<Photo> GetPaginationPhotosByDescriptionAndUserId(string description, int skip, int take, Guid userId);
    }
}
