using WebStore.Models.ValueObjects;

namespace WebStore.Models
{
    public class Customer
    {
        public DateTime DateOfBirth { get; set; }
        
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        
        public List<Address> ShippingAddress { get; set; } = new();
    }
}

