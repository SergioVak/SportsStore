using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Count_Of_Added_Lines_Is_2()
        {
            //Организация
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "Р2" };

            Cart target = new Cart();

            //Действие
            target.Additem(p1, 1);
            target.Additem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            //Утверждение
            Assert.Equal(2, results.Length);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Организация
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "Р2" };

            Cart target = new Cart();

            //Действие
            target.Additem(p1, 1);
            target.Additem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            //Утверждение
            Assert.Equal(11, results[0].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //Организация
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "Р2" };
            Product p3 = new Product { ProductID = 3, Name = "Р3" };

            Cart target = new Cart();

            target.Additem(p1, 1);
            target.Additem(p2, 3);
            target.Additem(p3, 5);
            target.Additem(p2, 1);

            //Действие
            target.RemoveLine(p2);

            //Утверждение
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            //Организация
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "Р2", Price = 50M };

            Cart target = new Cart();

            target.Additem(p1, 1);
            target.Additem(p2, 1);
            target.Additem(p1, 3);

            decimal result = target.ComputeTotalValue();

            //Утверждение
            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            //Организация
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "Р2", Price = 50M };

            Cart target = new Cart();

            target.Additem(p1, 1);
            target.Additem(p2, 1);

            //Действие - очистка корзины
            target.Clear();

            //Утверждение
            Assert.Equal(0, target.Lines.Count());

        }
    }
}
