using System.ComponentModel.DataAnnotations;
using WebStore.Models.ValueObjects;

namespace WebStore.Models
{
    public class Customer
    {
        private static readonly List<Customer> _extent = new List<Customer>();

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

        [MinLength(0, ErrorMessage = "Shipping address list cannot be null")]
        public List<Address> ShippingAddress { get; set; } = new();

        public static List<Customer> GetAll()
        {
            return new List<Customer>(_extent);
        }

        public Customer()
        {
            _extent.Add(this);
        }
    }
}

