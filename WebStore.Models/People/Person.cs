namespace WebStore.Models
{
    public abstract class Person
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public static int LegalAdultAge { get; set; } = 18;
    }
}

