using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Count_On_Second_Page_Is_2()
        {
            //организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //действие

            ProductsListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            Product[] productArray = result.Products.ToArray();

            //утверждение
            Assert.True(productArray.Length == 2);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable<Product>());

            //Организация

            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };
            // Действие

            Product[] result =
                (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

            //Утверждение

            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
        }      
    }
}
