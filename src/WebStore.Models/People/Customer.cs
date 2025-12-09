using System.ComponentModel.DataAnnotations;
using WebStore.Models.ValueObjects;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Customer : Person
    {
        private static List<Customer> _extent = new List<Customer>();

        private DateTime _dateOfBirth;
        private readonly List<Order> _orders = new();

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value > DateTime.Today)
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), 
                        "Date of birth cannot be in the future");
                if (value < DateTime.Today.AddYears(-150))
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), 
                        "Date of birth cannot be more than 150 years ago");
                _dateOfBirth = value;
            }
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [Required(ErrorMessage = "Shipping address list cannot be null")]
        public List<Address> ShippingAddress { get; set; } = new();

        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        public static List<Customer> GetAll()
        {
            return new List<Customer>(_extent);
        }

        internal void AddOrderInternal(Order order) => LinkOrder(order);

        internal void RemoveOrderInternal(Order order) => UnlinkOrder(order);

        internal void AddOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            order.SetCustomerInternal(this);
        }

        internal void RemoveOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (!_orders.Contains(order))
                throw new InvalidOperationException("Order is not associated with this customer.");

            throw new InvalidOperationException("Cannot remove customer from order because an order must always have an associated customer.");
        }
    
        public new static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Customers", directory);
        }
        
        public new static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Customers", directory))
                return;

            var loadedCustomers = XmlPersistenceService.LoadFromXml<Customer>("Customers", directory);
            
            _extent.Clear();
            foreach (var customer in loadedCustomers)
            {
                _extent.Add(customer);
            }
        }
        
        public Customer()
        {
        }

        public Customer(string firstName, string lastName, string phoneNumber, DateTime dateOfBirth) 
            : base(firstName, lastName, phoneNumber)
        {
            DateOfBirth = dateOfBirth;
            _extent.Add(this);
        }

        private void LinkOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (_orders.Contains(order))
                return;

            _orders.Add(order);
            order.SetCustomerInternal(this);
        }

        private void UnlinkOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (!_orders.Remove(order))
                return;

            order.RemoveCustomerInternal(this);
        }
    }
}

