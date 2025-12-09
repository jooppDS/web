using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public abstract class Product
    {
        private static List<Product> _extent = new List<Product>();

        private string _name = string.Empty;
        private string _description = string.Empty;
        private decimal _price;
        private bool _isAdultProduct;
        private static decimal _storeFeePercentage = 5;
        private decimal _weight;
        private int _stockQuantity;
        private Seller _seller = null!;
        private readonly List<ProductInOrder> _productsInOrder = new();

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or empty", nameof(Name));
                if (value.Length < 2 || value.Length > 100)
                    throw new ArgumentException("Name must be between 2 and 100 characters", nameof(Name));
                bool nameExists = _extent.Any(p => p != this && 
                                                   string.Equals(p.Name, value, StringComparison.OrdinalIgnoreCase));
                if (nameExists)
                    throw new InvalidOperationException("A product with this name already exists.");
                _name = value;
            }
        }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters")]
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be null or empty", nameof(Description));
                if (value.Length < 10 || value.Length > 1000)
                    throw new ArgumentException("Description must be between 10 and 1000 characters", nameof(Description));
                _description = value;
            }
        }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Price), 
                        "Price cannot be negative");
                _price = value;
            }
        }

        public bool IsAdultProduct
        {
            get => _isAdultProduct;
            set => _isAdultProduct = value;
        }

        [Range(0, 100, ErrorMessage = "Store fee percentage must be between 0 and 100")]
        public decimal StoreFeePercentage
        {
            get => _storeFeePercentage;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(StoreFeePercentage), 
                        "Store fee percentage must be between 0 and 100");
                _storeFeePercentage = value;
            }
        }

        [Required(ErrorMessage = "Weight is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be non-negative")]
        public decimal Weight
        {
            get => _weight;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Weight), 
                        "Weight cannot be negative");
                _weight = value;
            }
        }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be non-negative")]
        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(StockQuantity), 
                        "Stock quantity cannot be negative");
                _stockQuantity = value;
            }
        }

        [Required(ErrorMessage = "Seller is required")]
        public Seller Seller
        {
            get => _seller;
            private set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Seller), "Seller cannot be null");

                if (ReferenceEquals(_seller, value))
                    return;

                var oldSeller = _seller;
                _seller = value;

                if (oldSeller != null)
                {
                    oldSeller.RemoveProductInternal(this);
                }

                value.AddProductInternal(this);
            }
        }

        internal void ChangeSellerInternal(Seller newSeller)
        {
            Seller = newSeller;
        }

        public void ChangeSeller(Seller newSeller)
        {
            if (newSeller is null)
                throw new ArgumentNullException(nameof(newSeller));

            if (_seller == newSeller)
                return;
            
            ChangeSellerInternal(newSeller);
            newSeller.AddProductInternal(this);
        }
        
        public IReadOnlyCollection<ProductInOrder> ProductsInOrder => _productsInOrder.AsReadOnly();

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

        public ProductInOrder AddProductToOrder(Order order, int quantity)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            
            return new ProductInOrder(this, order, quantity);
        }
        
        public void RemoveProductInOrder(ProductInOrder productInOrder)
        {
            if (productInOrder is null)
                throw new ArgumentNullException(nameof(productInOrder));

            if (!_productsInOrder.Contains(productInOrder))
                throw new InvalidOperationException("Given product line is not part of this order.");

            productInOrder.Delete();
        }

        public static List<Product> GetAll()
        {
            return new List<Product>(_extent);
        }

        public void Delete()
        {
            var items = new List<ProductInOrder>(_productsInOrder);
            foreach (var productInOrder in items)
            {
                productInOrder.Delete();
            }

            _seller.RemoveProductInternal(this);
            _extent.Remove(this);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Products", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Products", directory))
                return;

            var loadedProducts = XmlPersistenceService.LoadFromXml<Product>("Products", directory);

            _extent.Clear();
            foreach (var product in loadedProducts)
            {
                _extent.Add(product);
            }
        }

        protected Product()
        {
        }

        protected Product(string name, string description, decimal price, bool isAdultProduct, decimal weight, int stockQuantity, Seller seller)
        {
            Name = name;
            Description = description;
            Price = price;
            IsAdultProduct = isAdultProduct;
            Weight = weight;
            StockQuantity = stockQuantity;
            Seller = seller ?? throw new ArgumentNullException(nameof(seller));
            _extent.Add(this);
        }
    }
}
