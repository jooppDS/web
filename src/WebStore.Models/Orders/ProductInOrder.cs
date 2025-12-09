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

        internal void SetProductInternal(Product product) => LinkProduct(product);

        internal void ClearProductInternal(Product product) => UnlinkProduct(product);

        internal void SetOrderInternal(Order order) => LinkOrder(order);

        internal void ClearOrderInternal(Order order) => UnlinkOrder(order);

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
            LinkProduct(product ?? throw new ArgumentNullException(nameof(product)));
            LinkOrder(order ?? throw new ArgumentNullException(nameof(order)));
            Quantity = quantity;
            _extent.Add(this);
        }

        public void ChangeQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void Delete(bool forceDelete = false)
        {
            if (forceDelete == false && OrderItemsCountForOrderIsOne())
                throw new InvalidOperationException("Cannot remove the last product from an order. An order must contain at least one product.");

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
        
        private bool OrderItemsCountForOrderIsOne()
        {
            return Order.GetProductInOrdersInternalCount() == 1;
        }

        private void LinkProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (ReferenceEquals(_product, product))
                return;

            var oldProduct = _product;
            _product = product;

            product.AddProductInOrderInternal(this);

            if (oldProduct != null && !ReferenceEquals(oldProduct, product))
            {
                oldProduct.RemoveProductInOrderInternal(this);
            }
        }

        private void UnlinkProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (!ReferenceEquals(_product, product))
                return;

            _product = null!;
            product.RemoveProductInOrderInternal(this);
        }

        private void LinkOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (ReferenceEquals(_order, order))
                return;

            var oldOrder = _order;
            _order = order;

            order.AddProductInOrderInternal(this);

            if (oldOrder != null && !ReferenceEquals(oldOrder, order))
            {
                oldOrder.RemoveProductInOrderInternal(this);
            }
        }

        private void UnlinkOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (!ReferenceEquals(_order, order))
                return;

            _order = null!;
            order.RemoveProductInOrderInternal(this);
        }
    }
}

