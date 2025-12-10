using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Clothing : Product
    {
        private static List<Clothing> _extent = new List<Clothing>();

        private List<string> _materials = new List<string>();
        private ClothingSize _size;
        private Gender _gender;
        private string _careInstruction = string.Empty;
        private readonly List<Clothing> _relatedClothing = new();

        [Required(ErrorMessage = "Materials are required")]
        [MinLength(1, ErrorMessage = "At least one material is required")]
        public List<string> Materials
        {
            get => _materials;
            set
            {
                if (value == null || value.Count == 0)
                    throw new ArgumentException("At least one material is required", nameof(Materials));
                _materials = value;
            }
        }

        [Required(ErrorMessage = "Size is required")]
        public ClothingSize Size
        {
            get => _size;
            set
            {
                if (!Enum.IsDefined(typeof(ClothingSize), value))
                    throw new ArgumentOutOfRangeException(nameof(Size), 
                        "Size must be a valid ClothingSize value");
                _size = value;
            }
        }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender
        {
            get => _gender;
            set
            {
                if (!Enum.IsDefined(typeof(Gender), value))
                    throw new ArgumentOutOfRangeException(nameof(Gender), 
                        "Gender must be a valid Gender value");
                _gender = value;
            }
        }

        [Required(ErrorMessage = "Care instruction is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Care instruction must be between 5 and 500 characters")]
        public string CareInstruction
        {
            get => _careInstruction;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Care instruction cannot be null or empty", nameof(CareInstruction));
                if (value.Length < 5 || value.Length > 500)
                    throw new ArgumentException("Care instruction must be between 5 and 500 characters", nameof(CareInstruction));
                _careInstruction = value;
            }
        }

        public IReadOnlyCollection<Clothing> RelatedClothing => _relatedClothing.AsReadOnly();

    
        public static List<Clothing> GetAll()
        {
            return new List<Clothing>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Clothing", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Clothing", directory))
                return;

            var loadedClothing = XmlPersistenceService.LoadFromXml<Clothing>("Clothing", directory);

            _extent.Clear();
            foreach (var item in loadedClothing)
            {
                _extent.Add(item);
            }
        }

        public Clothing()
        {
            _materials = new List<string>();
        }

        public Clothing(string name, string description, decimal price, bool isAdultProduct, decimal weight, int stockQuantity, 
            List<string> materials, ClothingSize size, Gender gender, string careInstruction, Seller seller)
            : base(name, description, price, isAdultProduct, weight, stockQuantity, seller)
        {
            Materials = materials;
            Size = size;
            Gender = gender;
            CareInstruction = careInstruction;
            _extent.Add(this);
        }

        public new void Delete()
        {
            base.Delete();
            var items = new List<Clothing>(_relatedClothing);
            foreach (var clothing in items)
            {
                clothing.RemoveRelatedClothing(this);
            }

            _extent.Remove(this);
        }

        public void AddRelatedClothing(Clothing other) => LinkRelatedClothing(other);

        public void RemoveRelatedClothing(Clothing other) => UnlinkRelatedClothing(other);

        private void LinkRelatedClothing(Clothing other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (ReferenceEquals(this, other))
                throw new ArgumentException("Clothing cannot associate with itself", nameof(other));

            if (_relatedClothing.Contains(other))
                return;

            _relatedClothing.Add(other);
            if (!other._relatedClothing.Contains(this))
            {
                other.AddRelatedClothing(this);
            }
        }

        private void UnlinkRelatedClothing(Clothing other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (!_relatedClothing.Remove(other))
                return;

            if (other._relatedClothing.Contains(this))
            {
                other.RemoveRelatedClothing(this);
            }
        }
    }
}
