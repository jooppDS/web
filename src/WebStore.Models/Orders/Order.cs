using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Order
    {
        private static List<Order> _extent = new List<Order>();

        private DateTime _date;
        private OrderStatus _status;
        private DeliveryType _deliveryType;
        private bool _isHidden;
        private Customer _customer = null!;
        private readonly List<ProductInOrder> _productsInOrder = new();

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        [Required(ErrorMessage = "Status is required")]
        public OrderStatus Status
        {
            get => _status;
            set
            {
                if (!Enum.IsDefined(typeof(OrderStatus), value))
                    throw new ArgumentOutOfRangeException(nameof(Status), 
                        "Status must be a valid OrderStatus value");
                _status = value;
            }
        }

        [Required(ErrorMessage = "Delivery type is required")]
        public DeliveryType DeliveryType
        {
            get => _deliveryType;
            set
            {
                if (!Enum.IsDefined(typeof(DeliveryType), value))
                    throw new ArgumentOutOfRangeException(nameof(DeliveryType), 
                        "DeliveryType must be a valid DeliveryType value");
                _deliveryType = value;
            }
        }

        public bool IsHidden
        {
            get => _isHidden;
            private set => _isHidden = value;
        }

        [Required(ErrorMessage = "Customer is required")]
        public Customer Customer
        {
            get => _customer;
            private set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Customer), "Customer cannot be null");

                if (ReferenceEquals(_customer, value))
                    return;

                var oldCustomer = _customer;
                _customer = value;

                if (oldCustomer != null)
                {
                    oldCustomer.RemoveOrderInternal(this);
                }

                value.AddOrderInternal(this);
            }
        }

        public IReadOnlyCollection<ProductInOrder> ProductsInOrder => _productsInOrder.AsReadOnly();

        internal void ChangeCustomerInternal(Customer customer)
        {
            Customer = customer;
        }
        
        public void ChangeCustomer(Customer customer)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));
            
            if (_customer == customer)
                return;
            
            ChangeCustomerInternal(customer);
            customer.AddOrder(this);
        }

        public void ChangeVisibility(bool isHidden)
        {
            IsHidden = isHidden;
        }

        internal void AddProductInOrderInternal(ProductInOrder productInOrder)
        {
            if (!_productsInOrder.Contains(productInOrder))
            {
                _productsInOrder.Add(productInOrder);
            }
        }

        internal void RemoveProductInOrderInternal(ProductInOrder productInOrder)
        {
            _productsInOrder.Remove(productInOrder);
        }

        internal int GetProductInOrdersInternalCount()
        {
            return _productsInOrder.Count;
        }

        public ProductInOrder AddProduct(Product product, int quantity)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            return new ProductInOrder(product, this, quantity);
        }

        public void RemoveProductInOrder(ProductInOrder productInOrder)
        {
            if (productInOrder is null)
                throw new ArgumentNullException(nameof(productInOrder));

            if (!_productsInOrder.Contains(productInOrder))
                throw new InvalidOperationException("Given product line is not part of this order.");

            productInOrder.Delete();
        }

        public static List<Order> GetAll()
        {
            return new List<Order>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Orders", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Orders", directory))
                return;

            var loadedOrders = XmlPersistenceService.LoadFromXml<Order>("Orders", directory);

            _extent.Clear();
            foreach (var order in loadedOrders)
            {
                _extent.Add(order);
            }
        }

        public Order()
        {
        }

        public Order(DateTime date, OrderStatus status, DeliveryType deliveryType, Customer customer, bool isHidden = false)
        {
            Date = date;
            Status = status;
            DeliveryType = deliveryType;
            ChangeCustomer(customer);
            IsHidden = isHidden;
            _extent.Add(this);
        }

        public void Delete()
        {
            foreach (var productInOrder in _productsInOrder.ToList())
            {
                productInOrder.Delete(true);
            }

            _customer.RemoveOrderInternal(this);
            _extent.Remove(this);
        }
    }
}
