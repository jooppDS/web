using WebStore.Models.ValueObjects;

namespace WebStore.Models
{
    public class Manufacturer
    {
        public string Name { get; set; } = string.Empty;
        public Address Address { get; set; } = new();
    }
}

