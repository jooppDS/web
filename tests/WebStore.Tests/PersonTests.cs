using WebStore.Models;

namespace WebStore.Tests;

public class PersonTests
{
    private class TestPerson : Person
    {
        public TestPerson(string firstName, string lastName, string phoneNumber)
            : base(firstName, lastName, phoneNumber)
        {
        }
    }
    
    [Test]
    public void PersonCreatedProperly()
    {
        var initialCount = Person.GetAll().Count;

        var person = new TestPerson("Yehor", "Lopasov", "+123456789");
        Assert.That(Person.GetAll().Count, Is.EqualTo(initialCount + 1));
        Assert.That(person.FirstName, Is.EqualTo("Yehor"));
        Assert.That(person.LastName, Is.EqualTo("Lopasov"));
        Assert.That(person.PhoneNumber, Is.EqualTo("+123456789"));
    }
    
    [Test]
    public void InvalidFirstNameThrowsArgumentException()
    {
        var person = new TestPerson("Yehor", "Lopasov", "+123456789");
        Assert.Throws<ArgumentException>(() => person.FirstName = "");
        Assert.Throws<ArgumentException>(() => person.FirstName = null);
        Assert.Throws<ArgumentException>(() => person.FirstName = "A");
        Assert.Throws<ArgumentException>(() => person.FirstName = new string('a', 51));
    }
    
    [Test]
    public void InvalidLastNameThrowsArgumentException()
    {
        var person = new TestPerson("Yehor", "Lopasov", "+123456789");
        Assert.Throws<ArgumentException>(() => person.LastName = "");
        Assert.Throws<ArgumentException>(() => person.LastName = null);
        Assert.Throws<ArgumentException>(() => person.LastName = "A");
        Assert.Throws<ArgumentException>(() => person.LastName = new string('a', 51));
    }
    
    [Test]
    public void InvalidPhoneNumberThrowsArgumentException()
    {
        var person = new TestPerson("Yehor", "Lopasov", "+123456789");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = null);
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "abc123");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "+012345");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "+12345678901234567");
    }
}