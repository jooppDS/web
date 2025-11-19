using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class ProductInOrder
    {
        private static readonly List<ProductInOrder> _extent = new();
        private int _quantity = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity must be at least 1");
                _quantity = value;
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

        public ProductInOrder(int quantity)
        {
            Quantity = quantity;
            _extent.Add(this);
        }

        public void EnsureCustomerCanPurchase(int customerAge, int legalAdultAge)
        {
            if (customerAge < 0)
                throw new ArgumentOutOfRangeException(nameof(customerAge));
            if (legalAdultAge < 0)
                throw new ArgumentOutOfRangeException(nameof(legalAdultAge));
        }
    }
}

