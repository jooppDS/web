using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;
using WebStore.Models.ValueObjects;

namespace WebStore.Models
{
    public class Seller
    {
        private static List<Seller> _extent = new List<Seller>();

        private string _name = string.Empty;
        private Address _address = null!;

        [Required(ErrorMessage = "Seller name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Seller name must be between 2 and 100 characters")]
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Seller name cannot be null or empty", nameof(Name));
                if (value.Length < 2 || value.Length > 100)
                    throw new ArgumentException("Seller name must be between 2 and 100 characters", nameof(Name));
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

        public static List<Seller> GetAll()
        {
            return new List<Seller>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Sellers", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Sellers", directory))
                return;

            var loadedSellers = XmlPersistenceService.LoadFromXml<Seller>("Sellers", directory);

            _extent.Clear();
            foreach (var seller in loadedSellers)
            {
                _extent.Add(seller);
            }
        }

        public Seller()
        {
        }

        public Seller(string name, Address address)
        {
            Name = name;
            Address = address;
            _extent.Add(this);
        }
    }
}
