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
                if (value == OrderStatus.Cancelled)
                    Delete();
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
            private set => LinkCustomer(value ?? throw new ArgumentNullException(nameof(Customer), "Customer cannot be null"));
        }
        
        public IReadOnlyCollection<ProductInOrder> ProductsInOrder => _productsInOrder.AsReadOnly();

        public void HideOrder()
        {
            IsHidden = true;
        }

        public int GetProductInOrdersCount()
        {
            return _productsInOrder.Count;
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

        public Order(DateTime date, OrderStatus status, DeliveryType deliveryType, Customer customer)
        {
            Date = date;
            Status = status;
            DeliveryType = deliveryType;
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            IsHidden = false;
            _extent.Add(this);
        }

        public void Delete()
        {
            foreach (var productInOrder in _productsInOrder.ToList())
            {
                productInOrder.Delete();
            }

            if (_customer != null)
            {
                RemoveCustomer(_customer);
            }

            _extent.Remove(this);
        }


        public void AddCustomer(Customer customer) => LinkCustomer(customer);

        public void RemoveCustomer(Customer customer) => UnlinkCustomer(customer);

        private void LinkCustomer(Customer customer)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            if (ReferenceEquals(_customer, customer))
                return;

            var oldCustomer = _customer;
            _customer = customer;

            customer.AddOrder(this);

            if (oldCustomer != null && !ReferenceEquals(oldCustomer, customer))
            {
                oldCustomer.RemoveOrder(this, true);
            }
        }

        private void UnlinkCustomer(Customer customer)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            if (!ReferenceEquals(_customer, customer))
                return;

            _customer = null!;
            customer.RemoveOrder(this, true);
        }
        
        public void AddProductInOrder(ProductInOrder productInOrder) => LinkProductInOrder(productInOrder);

        public void RemoveProductInOrder(ProductInOrder productInOrder) => UnlinkProductInOrder(productInOrder);

        private void LinkProductInOrder(ProductInOrder productInOrder)
        {
            if (productInOrder is null)
                throw new ArgumentNullException(nameof(productInOrder));

            if (productInOrder.Product.IsAdultProduct && _customer.Age < Person.LegalAdultAge)
                throw new ArgumentException("Customer does not meet the age requirement");
            
            if (_productsInOrder.Contains(productInOrder))
                return;

            _productsInOrder.Add(productInOrder);
        }

        private void UnlinkProductInOrder(ProductInOrder productInOrder)
        {
            if (productInOrder is null)
                throw new ArgumentNullException(nameof(productInOrder));
            
            if (!_productsInOrder.Contains(productInOrder))
                throw new InvalidOperationException("Given product line is not part of this order.");

            _productsInOrder.Remove(productInOrder);
            productInOrder.Delete();
        }
    }
}
