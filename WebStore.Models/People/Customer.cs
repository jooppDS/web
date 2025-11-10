using WebStore.Models.ValueObjects;

namespace WebStore.Models
{
    public class Customer
    {
        public DateTime DateOfBirth { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
        public List<Address> ShippingAddress { get; set; } = new();
    }
}

