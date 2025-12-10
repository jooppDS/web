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
            private set => LinkProduct(value ?? throw new ArgumentNullException(nameof(Product), "Product cannot be null"));
        }

        [Required(ErrorMessage = "Order is required")]
        public Order Order
        {
            get => _order;
            private set => LinkOrder(value ?? throw new ArgumentNullException(nameof(Order), "Order cannot be null"));
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
            if (product == null) 
                throw new ArgumentNullException(nameof(product));
            if (order == null) 
                throw new ArgumentNullException(nameof(order));
            
            var duplicateExists = _extent.Any(pio =>
                ReferenceEquals(pio.Product, product) &&
                ReferenceEquals(pio.Order, order));

            if (duplicateExists)
                throw new InvalidOperationException($"A ProductInOrder with product '{product}' and order '{order}' already exists.");
            
            LinkProduct(product);
            try
            {
                LinkOrder(order);
            }
            catch (ArgumentException ex)
            {
                product.RemoveProductInOrder(this);
                throw;
            }


            Quantity = quantity;
            _extent.Add(this);
        }

        public void ChangeQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void Delete(bool forceDelete = false)
        {
            _extent.Remove(this);

            var product = _product;
            var order = _order;

            if (product != null)
            {
                UnlinkProduct(product);
            }

            if (order != null)
            {
                UnlinkOrder(order);
            }
        }

        private void LinkProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (ReferenceEquals(_product, product))
                return;

            var oldProduct = _product;
            _product = product;

            if (!product.ProductsInOrder.Contains(this))
            {
                product.AddProductInOrder(this);
            }

            if (oldProduct != null && !ReferenceEquals(oldProduct, product))
            {
                oldProduct.RemoveProductInOrder(this);
            }
        }

        private void UnlinkProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (!ReferenceEquals(_product, product))
                return;

            _product = null!;
            if (product.ProductsInOrder.Contains(this))
            {
                product.RemoveProductInOrder(this);
            }
        }

        private void LinkOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (ReferenceEquals(_order, order))
                return;

            var oldOrder = _order;
            _order = order;

            if (!order.ProductsInOrder.Contains(this))
            {
                order.AddProductInOrder(this);
            }

            if (oldOrder != null && !ReferenceEquals(oldOrder, order))
            {
                oldOrder.RemoveProductInOrder(this);
            }
        }

        private void UnlinkOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (!ReferenceEquals(_order, order))
                return;

            _order = null!;
            if (order.ProductsInOrder.Contains(this))
            {
                order.RemoveProductInOrder(this);
            }
        }
    }
}

