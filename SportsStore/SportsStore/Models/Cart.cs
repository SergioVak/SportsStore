using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> _lineCollection;

        public Cart()
        {
            _lineCollection = new List<CartLine>();
        }

        public virtual void Additem(Product product, int quantity)
        {
            CartLine line = _lineCollection
                 .Where(p => p.Product.ProductID == product.ProductID)
                 .FirstOrDefault();

            if (line == null)
            {
                _lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            _lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public virtual decimal ComputeTotalValue() =>
            _lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => _lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => _lineCollection;
    }
}
