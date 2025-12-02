using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class ProductInOrder
    {
        private static readonly List<ProductInOrder> _extent = new();

        private int _quantity = 1;
        private Product _product = null!;
        private Order _order = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity
        {
            get => _quantity;
            private set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity must be at least 1");
                _quantity = value;
            }
        }

        [Required(ErrorMessage = "Product is required")]
        public Product Product
        {
            get => _product;
            private set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Product), "Product cannot be null");

                if (ReferenceEquals(_product, value))
                    return;

                var oldProduct = _product;
                _product = value;

                if (oldProduct != null)
                {
                    oldProduct.RemoveProductInOrderInternal(this);
                }

                value.AddProductInOrderInternal(this);
            }
        }

        [Required(ErrorMessage = "Order is required")]
        public Order Order
        {
            get => _order;
            private set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Order), "Order cannot be null");

                if (ReferenceEquals(_order, value))
                    return;

                var oldOrder = _order;
                _order = value;

                if (oldOrder != null)
                {
                    oldOrder.RemoveProductInOrderInternal(this);
                }

                value.AddProductInOrderInternal(this);
            }
        }

        public static IReadOnlyCollection<ProductInOrder> GetAll()
        {
            return _extent.AsReadOnly();
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "ProductsInOrders", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("ProductsInOrders", directory))
                return;

            var loadedItems = XmlPersistenceService.LoadFromXml<ProductInOrder>("ProductsInOrders", directory);

            _extent.Clear();
            foreach (var item in loadedItems)
            {
                _extent.Add(item);
            }
        }

        public ProductInOrder()
        {
        }

        public ProductInOrder(Product product, Order order, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Quantity = quantity;
            _extent.Add(this);
        }

        public void ChangeQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void ChangeProduct(Product product)
        {
            Product = product;
        }

        public void ChangeOrder(Order order)
        {
            Order = order;
        }

        public void Delete()
        {
            if (OrderItemsCountForOrderIsOne())
                throw new InvalidOperationException("Cannot remove the last product from an order. An order must contain at least one product.");

            _extent.Remove(this);
            _product.RemoveProductInOrderInternal(this);
            _order.RemoveProductInOrderInternal(this);
        }

        private bool OrderItemsCountForOrderIsOne()
        {
            return Order.GetProductInOrdersInternalCount() == 1;
        }
    }
}

