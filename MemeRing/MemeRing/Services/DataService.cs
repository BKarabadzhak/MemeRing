using MemeRing.Contexts;
using MemeRing.Models;
using MemeRing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MemeRing.Services
{
    /// <summary>
    /// The Data service class.
    /// Handles procedures with database.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly DataContext _context;

        public DataService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add specific entity to database.
        /// </summary>
        /// <param name="entity">Entity to be added.</param>
        public void Add<T>(T entity)
        {
            _context.Add(entity);
        }


        /// <summary>
        /// Delete specific entity from database.
        /// </summary>
        /// <param name="entity">Entity to be deleted.</param>
        public void Delete<T>(T entity)
        {
            _context.Remove(entity);
        }


        /// <summary>
        /// Update specific entity in database.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        public void Update<T>(T entity)
        {
            _context.Update(entity);
        }


        /// <summary>
        /// Save all changes in database.
        /// </summary>
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }


        /// <summary>
        /// Get specific photo by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> photo id.</param>
        /// <returns>Returns object <see cref="Photo"/>.</returns>
        public Photo GetPhoto(Guid id)
        {
            return _context.Photos.FirstOrDefault(p => p.Id == id);
        }


        /// <summary>
        /// Get collection of pagination photos of specific user by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> photo id.</param>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        public IEnumerable<Photo> GetPaginationPhotosByUserId(Guid id, int skip, int take)
        {
            return _context.Photos.Where(photo => photo.UserId == id).Skip(skip).Take(take).ToList();
        }


        /// <summary>
        /// Get specific user by its id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> user id.</param>
        /// <returns>Returns object <see cref="User"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when user doesn`t exist in system.</exception>
        public User GetUser(Guid id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    throw new InvalidOperationException("User doesn`t exist.");
                }

                return user;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Get collection of all users from the system.
        /// </summary>
        /// <returns>Returns collection of objects <see cref="User"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the system doesn`t contain users.</exception>
        public IEnumerable<User> GetUsers()
        {
            try
            {
                var users = _context.Users.ToList();

                if (users.Count() == 0)
                {
                    throw new InvalidOperationException();
                }

                return users;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
        }


        /// <summary>
        /// Get collection of pagination photos by their description.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        public IEnumerable<Photo> GetPaginationPhotosByDescription(string description, int skip, int take)
        {
            return (from entity in _context.Photos
                    where EF.Functions.Like(entity.Description, $"%{description}%")
                    select entity).Skip(skip).Take(take).ToList();
        }


        /// <summary>
        /// Get collection of pagination photos by description and <see cref="Guid"/> user id.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <param name="id"><see cref="Guid"/> user id.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        public IEnumerable<Photo> GetPaginationPhotosByDescriptionAndUserId(string description, int skip, int take, Guid id)
        {
            return (from entity in _context.Photos
                    where EF.Functions.Like(entity.Description, $"%{description}%")
                    where entity.UserId == id
                    select entity).Skip(skip).Take(take).ToList();
        }


        /// <summary>
        /// Get count of photos by their description.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        public int GetCountPhotosByDescription(string description)
        {
            return (from entity in _context.Photos
                    where EF.Functions.Like(entity.Description, $"%{description}%")
                    select entity).Count();
        }


        /// <summary>
        /// Get count of photos by their description and user id.
        /// </summary>
        /// <param name="description">Description of photo.</param>
        /// <param name="userId"><see cref="Guid"/> user id.</param>
        public int GetCountPhotosByDescriptionAndUserId(string description, Guid userId)
        {
            return (from entity in _context.Photos
                    where EF.Functions.Like(entity.Description, $"%{description}%")
                    where entity.UserId == userId
                    select entity).Count();
        }


        /// <summary>
        /// Get count of all photos in system.
        /// </summary>
        public int GetCountAllPhotos()
        {
            return (from entity in _context.Photos
                    select entity).Count();
        }


        /// <summary>
        /// Get count of photos by <see cref="Guid"/> user id.
        /// </summary>
        /// <param name="userId"><see cref="Guid"/> user id.</param>
        public int GetCountAllPhotosByUserId(Guid userId)
        {
            return (from entity in _context.Photos
                    where entity.UserId == userId
                    select entity).Count();
        }


        /// <summary>
        /// Get collection of pagination photos.
        /// </summary>
        /// <param name="skip">Number of photos to skip them.</param>
        /// <param name="take">Number of photos to take them.</param>
        /// <returns>Returns collection of objects <see cref="Photo"/>.</returns>
        public IEnumerable<Photo> GetPaginationPhotos(int skip, int take)
        {
            return _context.Photos.Skip(skip).Take(take);
        }


        /// <summary>
        /// Get all pages number.
        /// </summary>
        /// <param name="allPhotosNumber">Number of all photos.</param>
        /// <param name="pageSize">Count of photos on page.</param>
        public int GetAllPagesNumber(int allPhotosNumber, int pageSize)
        {
            return (int)Math.Ceiling(allPhotosNumber / (double)pageSize);
        }
    }
}
