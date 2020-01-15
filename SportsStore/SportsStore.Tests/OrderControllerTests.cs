using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{

    public class OrderControllerTests
    {

        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Arrange
            Cart cart = new Cart();
            // Arrange 
            Order order = new Order();
            // Arrange
            OrderController target = new OrderController(mock.Object, cart);

            // Act
            ViewResult result = target.Checkout(order) as ViewResult;

            // Assert
            Assert.False(result.ViewData.ModelState.IsValid);
        }
       
        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Arrange
            Cart cart = new Cart();
            cart.Additem(new Product(), 1);
            // Arrange
            OrderController target = new OrderController(mock.Object, cart);

            // Act
            RedirectToActionResult result =
                 target.Checkout(new Order()) as RedirectToActionResult;

            // Assert
            Assert.Equal("Completed", result.ActionName);
        }

    }
}