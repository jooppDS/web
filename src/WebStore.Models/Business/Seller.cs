using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;
using WebStore.Models.ValueObjects;

namespace WebStore.Models
{
    public class Seller
    {
        private static List<Seller> _extent = new List<Seller>();

        private readonly Dictionary<string, Product> _productsByName = new(StringComparer.OrdinalIgnoreCase);

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

        public IReadOnlyCollection<Product> Products => _productsByName.Values.ToList().AsReadOnly();
        
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

        internal void AddProductInternal(Product product) => LinkProduct(product);

        internal void RemoveProductInternal(Product product) => UnlinkProduct(product);

        internal void AddProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            product.SetSellerInternal(this);
        }

        internal void RemoveProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (!_productsByName.TryGetValue(product.Name, out var existing) || !ReferenceEquals(existing, product))
                return;

            product.Delete();
        }

        public void Delete()
        {
            var products = new List<Product>(Products);
            foreach (var product in products)
            {
                product.Delete();
            }

            _extent.Remove(this);
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

        private void LinkProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (_productsByName.TryGetValue(product.Name, out var existing))
            {
                if (ReferenceEquals(existing, product))
                    return;

                throw new InvalidOperationException("A different product with the same name is already associated with this seller.");
            }

            _productsByName[product.Name] = product;
            product.SetSellerInternal(this);
        }

        private void UnlinkProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (!_productsByName.TryGetValue(product.Name, out var existing) || !ReferenceEquals(existing, product))
                return;

            _productsByName.Remove(product.Name);
            product.RemoveSellerInternal(this);
        }
    }
}
