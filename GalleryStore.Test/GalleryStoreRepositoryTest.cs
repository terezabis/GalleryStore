using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using GalleryStore.Data;
using System;
using GalleryStore.Data.Entities;
using GalleryStore.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace GalleryStore.Test
{
    public class GalleryStoreRepositoryTest
    {
        [Fact]
        public void GetProductReturnCorrectProduct()
        {
            // Arrange
            var db = this.GetDatabase();
            var mock = new Mock<ILogger<GalleryStoreRepository>>();
            ILogger<GalleryStoreRepository> logger = mock.Object;


            var firstProduct = new Product { Id = 1, Title = "First", Price = 90 };
            var secondProduct = new Product { Id = 2, Title = "Second", Price = 24 };
            var thirdProduct = new Product { Id = 3, Title = "Third", Price = 53 };

            db.Add(firstProduct);
            db.Add(secondProduct);
            db.Add(thirdProduct);
            db.SaveChanges();

            var repository = new GalleryStoreRepository(db, logger);
            // Act
            var result = repository.GetProduct(2);
            // Assert
            result.Title.Should().BeSameAs("Second");
        }

        private GalleryStoreContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<GalleryStoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new GalleryStoreContext(dbOptions);
        }
    }
}
