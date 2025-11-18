using System.ComponentModel.DataAnnotations;
using WebStore.Models.ValueObjects;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Customer
    {
        private static List<Customer> _extent = new List<Customer>();

        private DateTime _dateOfBirth;

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

        public static List<Customer> GetAll()
        {
            return new List<Customer>(_extent);
        }

    
        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Customers", directory);
        }

        
        public static void LoadFromXml(string? directory = null)
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

        public Customer(DateTime dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
            _extent.Add(this);
        }
    }
}

