using System;
using Xunit;
using Moq;
using MemeRing.Services.Interfaces;
using MemeRing.Controllers;
using AutoMapper;
using MemeRing.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MemeRing.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using MemeRing.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IO;

namespace MemeRingTests
{
    public class PhotosControllerTests
    {
        [Fact]
        public void GetPhoto_InvalidPhotoId_ThrowInvalidOperationException()
        {
            var authServiceMock = new Mock<IAuthService>();
            var dataServiceMock = new Mock<IDataService>();

            Guid photoId = Guid.NewGuid();

            dataServiceMock.Setup(u => u.GetPhoto(photoId)).Throws(new InvalidOperationException("User hasn`t this photo."));

            var controller = new PhotosController(dataServiceMock.Object, authServiceMock.Object);

            var result = controller.GetPhoto(photoId);

            var statusCode = ((BadRequestObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((BadRequestObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 400);
            Assert.Equal("User hasn`t this photo.", dictionary["Message"]);

            dataServiceMock.VerifyAll();
        }


        [Fact]
        public void GetPaginationPhotosByDescription_InvalidDescription_ThrowInvalidOperationException()
        {
            var authServiceMock = new Mock<IAuthService>();
            var dataServiceMock = new Mock<IDataService>();

            var paginationPhotosByDescriptionRequestDto = new PaginationPhotosByDescriptionRequestDto() { Description = "description", PageNumber = 2, PageSize = 10 };

            dataServiceMock.Setup(u => u.GetPaginationPhotosByDescription(paginationPhotosByDescriptionRequestDto.Description,
                (paginationPhotosByDescriptionRequestDto.PageNumber - 1) * paginationPhotosByDescriptionRequestDto.PageSize, paginationPhotosByDescriptionRequestDto.PageSize))
                .Throws(new InvalidOperationException());

            var controller = new PhotosController(dataServiceMock.Object, authServiceMock.Object);

            var result = controller.GetPaginationPhotosByDescription(paginationPhotosByDescriptionRequestDto);

            var statusCode = ((NoContentResult)result).StatusCode;

            Assert.True(statusCode == 204);

            dataServiceMock.VerifyAll();
        }


        [Fact]
        public void GetPaginationPhotos_EmptyContainerOfPhotos_ThrowInvalidOperationException()
        {
            var authServiceMock = new Mock<IAuthService>();
            var dataServiceMock = new Mock<IDataService>();

            var paginationPhotosRequestDto = new PaginationPhotosRequestDto() { PageNumber = 2, PageSize = 10 };

            dataServiceMock.Setup(u => u.GetPaginationPhotos((paginationPhotosRequestDto.PageNumber - 1) * paginationPhotosRequestDto.PageSize,
                paginationPhotosRequestDto.PageSize))
                .Throws(new InvalidDataException("Photos don`t exist."));

            var controller = new PhotosController(dataServiceMock.Object, authServiceMock.Object);

            var result = controller.GetPaginationPhotos(paginationPhotosRequestDto);

            var statusCode = ((BadRequestObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((BadRequestObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 400);
            Assert.Equal("Photos don`t exist.", dictionary["Message"]);

            dataServiceMock.VerifyAll();
        }


        [Fact]
        public void GetAllUserPhotos_UserWithoutPhotos_ThrowInvalidOperationException()
        {
            var authServiceMock = new Mock<IAuthService>();
            var dataServiceMock = new Mock<IDataService>();

            Guid userId = Guid.NewGuid();

            var paginationPhotosRequestDto = new PaginationPhotosRequestDto() { PageNumber = 2, PageSize = 10 };

            dataServiceMock.Setup(u => u.GetPaginationPhotosByUserId(userId,
                (paginationPhotosRequestDto.PageNumber - 1) * paginationPhotosRequestDto.PageSize, paginationPhotosRequestDto.PageSize))
                .Throws(new InvalidOperationException());

            var controller = new PhotosController(dataServiceMock.Object, authServiceMock.Object);

            var context = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userId.ToString())
                    }, "Bearer"))
                }
            };

            controller.ControllerContext = context;

            var result = controller.GetAllUserPaginationPhotos(paginationPhotosRequestDto);

            var statusCode = ((NoContentResult)result).StatusCode;

            Assert.True(statusCode == 204);

            dataServiceMock.VerifyAll();
        }


        [Fact]
        public void UploadPhoto_ThrowException()
        {
            var authServiceMock = new Mock<IAuthService>();
            var dataServiceMock = new Mock<IDataService>();

            Guid userId = Guid.NewGuid();

            var fileToUploadDto = new FileToUploadDto() { FileName = "newPhoto.jpg", Description = "New photo.", Content = "base64Format" };

            authServiceMock.Setup(u => u.UserExists(userId)).Returns(true);
            dataServiceMock.Setup(u => u.SaveAll()).Throws(new Exception("Cannot save changes in database."));

            var controller = new PhotosController(dataServiceMock.Object, authServiceMock.Object);

            var context = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userId.ToString())
                    }, "Bearer"))
                }
            };

            controller.ControllerContext = context;

            var result = controller.UploadFile(fileToUploadDto);

            var statusCode = ((ObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((ObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 500);
            Assert.Equal("Cannot save changes in database.", dictionary["Error"]);

            authServiceMock.VerifyAll();
            dataServiceMock.VerifyAll();
        }


        [Fact]
        public void GetPhotoObject_PhotoId_ReturnPhoto()
        {
            var authServiceMock = new Mock<IAuthService>();
            var dataServiceMock = new Mock<IDataService>();

            Guid photoId = Guid.NewGuid();

            var photoObject = new Photo()
            {
                Id = Guid.NewGuid(),
                Description = "New photo.",
                Path = "C:\\Users\\Intern\\Downloads\\newPhoto.jpeg",
                UserId = Guid.NewGuid()
            };

            dataServiceMock.Setup(u => u.GetPhoto(photoObject.Id)).Returns(photoObject);

            var controller = new PhotosController(dataServiceMock.Object, authServiceMock.Object);

            var context = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, photoObject.UserId.ToString())
                    }, "Bearer"))
                }
            };

            controller.ControllerContext = context;

            var result = controller.GetPhotoObject(photoObject.Id);

            var statusCode = ((ObjectResult)result).StatusCode;
            var jsonValue = JsonConvert.SerializeObject(((ObjectResult)result).Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonValue);

            Assert.True(statusCode == 200);
            Assert.Equal(photoObject.Id, Guid.Parse(dictionary["Id"].ToString()));
            Assert.Equal(photoObject.Path, dictionary["Path"]);
            Assert.Equal(photoObject.Description, dictionary["Description"]);
            Assert.Equal(photoObject.UserId, Guid.Parse(dictionary["UserId"].ToString()));

            dataServiceMock.VerifyAll();
        }
    }
}
