using AutoMapper;
using FluentAssertions;
using GalleryStore.Controllers;
using GalleryStore.Data;
using GalleryStore.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GalleryStore.Test
{
    public class AppControllerTest
    {
        [Fact]
        public void ShopShouldReturnAllProducts()
        {
            var productRepository = new Mock<IGalleryStoreRepository>();
            productRepository.Setup(x => x.GetAllProducts()).Returns(new List<Product> { new Product(), new Product(), new Product() });
            var controller = new AppController(null, productRepository.Object, null);

            var result = controller.Shop();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());

        }

        [Fact]
        public void ShopShouldBeOnlyForAuthorize()
        {
            //Arrange
            var method = typeof(AppController)
                .GetMethod(nameof(AppController.Shop));

            //Act
            var attributes = method.GetCustomAttributes(true);

            //Assert
            attributes.Should().Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }
    }
}
