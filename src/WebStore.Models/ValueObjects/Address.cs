using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;

namespace WebStore.Models.ValueObjects
{
    public class Address
    {
        private static List<Address> _extent = new List<Address>();

        private string _street = string.Empty;
        private string _city = string.Empty;
        private string _state = string.Empty;
        private string _postalCode = string.Empty;
        private string _country = string.Empty;

        [Required(ErrorMessage = "Street is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Street must be between 1 and 100 characters")]
        public string Street
        {
            get => _street;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Street cannot be null or empty", nameof(Street));
                if (value.Length > 100)
                    throw new ArgumentException("Street cannot exceed 100 characters", nameof(Street));
                _street = value;
            }
        }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "City must be between 1 and 50 characters")]
        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("City cannot be null or empty", nameof(City));
                if (value.Length > 50)
                    throw new ArgumentException("City cannot exceed 50 characters", nameof(City));
                _city = value;
            }
        }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "State must be between 1 and 50 characters")]
        public string State
        {
            get => _state;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("State cannot be null or empty", nameof(State));
                if (value.Length > 50)
                    throw new ArgumentException("State cannot exceed 50 characters", nameof(State));
                _state = value;
            }
        }

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Postal code must be between 1 and 20 characters")]
        [RegularExpression(@"^[A-Z0-9\s-]+$", ErrorMessage = "Postal code must contain only letters, numbers, spaces, and hyphens")]
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Postal code cannot be null or empty", nameof(PostalCode));
                if (value.Length > 20)
                    throw new ArgumentException("Postal code cannot exceed 20 characters", nameof(PostalCode));
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-Z0-9\s-]+$"))
                    throw new ArgumentException("Postal code must contain only letters, numbers, spaces, and hyphens", nameof(PostalCode));
                _postalCode = value;
            }
        }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Country must be between 1 and 50 characters")]
        public string Country
        {
            get => _country;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Country cannot be null or empty", nameof(Country));
                if (value.Length > 50)
                    throw new ArgumentException("Country cannot exceed 50 characters", nameof(Country));
                _country = value;
            }
        }

        public static List<Address> GetAll()
        {
            return new List<Address>(_extent);
        }

        
        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Addresses", directory);
        }

     
        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Addresses", directory))
                return;

            var loadedAddresses = XmlPersistenceService.LoadFromXml<Address>("Addresses", directory);
            
            
            _extent.Clear();
            foreach (var address in loadedAddresses)
            {
                _extent.Add(address);
            }
        }

     
        public Address()
        {
            
        }

        public Address(string street, string city, string state, string postalCode, string country)
        {
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
            _extent.Add(this);
        }
    }
}

