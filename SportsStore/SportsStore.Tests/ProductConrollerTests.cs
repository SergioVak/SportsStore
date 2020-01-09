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
        public void Can_Paginate()
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

            //утверждение

            Product[] productArray = result.Products.ToArray();
            Assert.True(productArray.Length == 2);
            Assert.Equal("P4", productArray[0].Name);
            Assert.Equal("P5", productArray[1].Name);
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

            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>());

            //Организация

            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };
            // Действие

            ProductsListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Утверждение

            Paginginfo pageinfo = result.Paginginfo;
            Assert.Equal(2, pageinfo.CurrentPage);
            Assert.Equal(3, pageinfo.ItemsPerPage);
            Assert.Equal(5, pageinfo.Totalitems);
            Assert.Equal(2, pageinfo.TotalPages);
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
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

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            Func<ViewResult, ProductsListViewModel> GetModel = (result =>
            result?.ViewData?.Model as ProductsListViewModel);

            //Действие

            int? res1 = GetModel(target.List("Cat1"))?.Paginginfo.Totalitems;
            int? res2 = GetModel(target.List("Cat2"))?.Paginginfo.Totalitems;
            int? res3 = GetModel(target.List("Cat3"))?.Paginginfo.Totalitems;
            int? resAll = GetModel(target.List(null))?.Paginginfo.Totalitems;

            //Утверждение

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
