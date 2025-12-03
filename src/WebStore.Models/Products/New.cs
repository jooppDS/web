using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class New : Product
    {
        private static List<New> _extent = new List<New>();

        private TimeSpan _warrantyPeriod;

        [Required(ErrorMessage = "Warranty period is required")]
        public TimeSpan WarrantyPeriod
        {
            get => _warrantyPeriod;
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(WarrantyPeriod), 
                        "Warranty period must be positive");
                _warrantyPeriod = value;
            }
        }

        public static List<New> GetAll()
        {
            return new List<New>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "NewProducts", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("NewProducts", directory))
                return;

            var loadedProducts = XmlPersistenceService.LoadFromXml<New>("NewProducts", directory);

            _extent.Clear();
            foreach (var product in loadedProducts)
            {
                _extent.Add(product);
            }
        }

        public New()
        {
        }

        public New(string name, string description, decimal price, bool isAdultProduct, decimal weight, int stockQuantity, 
            TimeSpan warrantyPeriod, Seller seller)
            : base(name, description, price, isAdultProduct, weight, stockQuantity, seller)
        {
            WarrantyPeriod = warrantyPeriod;
            _extent.Add(this);
        }
    }
}
