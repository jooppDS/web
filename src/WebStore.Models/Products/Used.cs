using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Used : Product
    {
        private static List<Used> _extent = new List<Used>();

        private ProductCondition _condition;
        private string _defectsDescription = string.Empty;

        [Required(ErrorMessage = "Condition is required")]
        public ProductCondition Condition
        {
            get => _condition;
            set
            {
                if (!Enum.IsDefined(typeof(ProductCondition), value))
                    throw new ArgumentOutOfRangeException(nameof(Condition), 
                        "Condition must be a valid ProductCondition value");
                _condition = value;
            }
        }

        [Required(ErrorMessage = "Defects description is required")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Defects description must be between 5 and 1000 characters")]
        public string DefectsDescription
        {
            get => _defectsDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Defects description cannot be null or empty", nameof(DefectsDescription));
                if (value.Length < 5 || value.Length > 1000)
                    throw new ArgumentException("Defects description must be between 5 and 1000 characters", nameof(DefectsDescription));
                _defectsDescription = value;
            }
        }

        public static List<Used> GetAll()
        {
            return new List<Used>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "UsedProducts", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("UsedProducts", directory))
                return;

            var loadedProducts = XmlPersistenceService.LoadFromXml<Used>("UsedProducts", directory);

            _extent.Clear();
            foreach (var product in loadedProducts)
            {
                _extent.Add(product);
            }
        }

        public Used()
        {
        }

        public Used(string name, string description, decimal price, bool isAdultProduct, decimal weight, int stockQuantity, 
            ProductCondition condition, string defectsDescription, Seller seller)
            : base(name, description, price, isAdultProduct, weight, stockQuantity, seller)
        {
            Condition = condition;
            DefectsDescription = defectsDescription;
            _extent.Add(this);
        }
    }
}
