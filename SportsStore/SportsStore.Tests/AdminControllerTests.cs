﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());
            // Arrange 
            AdminController target = new AdminController(mock.Object);
            // Action
            Product[] result
                = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();
            // Assert
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void Can_Edit_Second_Product()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            // Arrange
            AdminController target = new AdminController(mock.Object);

            // Act
            Product p1 = GetViewModel<Product>(target.Edit(1));
            Product p2 = GetViewModel<Product>(target.Edit(2));
            Product p3 = GetViewModel<Product>(target.Edit(3));

            // Assert
            Assert.Equal(2, p2.ProductID);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            // Arrange
            AdminController target = new AdminController(mock.Object);

            // Act
            Product result = GetViewModel<Product>(target.Edit(4));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            // Arrange
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            // Arrange
            Product product = new Product { Name = "Test" };

            // Act
            IActionResult result = target.Edit(product);

            // Assert 
            mock.Verify(m => m.SaveProduct(product));
            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Can_Show_Index_After_Changes()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            // Arrange
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            // Arrange
            Product product = new Product { Name = "Test" };

            // Act
            IActionResult result = target.Edit(product);

            // Assert
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange
            AdminController target = new AdminController(mock.Object);
            // Arrange
            Product product = new Product { Name = "Test" };
            // Arrange
            target.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = target.Edit(product);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            // Arrange
            Product prod = new Product { ProductID = 2, Name = "Test" };

            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                prod,
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            // Arrange
            AdminController target = new AdminController(mock.Object);

            // Act
            target.Delete(prod.ProductID);

            // Assert
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }


        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
