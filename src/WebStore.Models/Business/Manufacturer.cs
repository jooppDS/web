using System.ComponentModel.DataAnnotations;
using WebStore.Models.ValueObjects;

namespace WebStore.Models
{
    public class Manufacturer
    {
        private static readonly List<Manufacturer> _extent = new List<Manufacturer>();

        private string _name = string.Empty;
        private Address _address = null!;

        [Required(ErrorMessage = "Manufacturer name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Manufacturer name must be between 2 and 100 characters")]
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Manufacturer name cannot be null or empty", nameof(Name));
                if (value.Length < 2 || value.Length > 100)
                    throw new ArgumentException("Manufacturer name must be between 2 and 100 characters", nameof(Name));
                _name = value;
            }
        }

        [Required(ErrorMessage = "Address is required")]
        public Address Address
        {
            get => _address;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Address), "Address cannot be null");
                _address = value;
            }
        }

        public static List<Manufacturer> GetAll()
        {
            return new List<Manufacturer>(_extent);
        }

        public Manufacturer(string name, Address address)
        {
            Name = name;
            Address = address;
            _extent.Add(this);
        }
    }
}

