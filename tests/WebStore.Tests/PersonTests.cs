using System.Reflection;
using WebStore.Models;

namespace WebStore.Tests;

[TestFixture]
public class PersonTests
{
    private class TestPerson : Person
    {
        public TestPerson(string firstName, string lastName, string phoneNumber)
            : base(firstName, lastName, phoneNumber)
        {
        }
    }
    
    [SetUp]
    public void SetUp()
    {
        var type = typeof(TestPerson);
        var extentField = type.GetField("_extent", BindingFlags.NonPublic | BindingFlags.Static);
        if (extentField != null)
        {
            var extent = extentField.GetValue(null);
            if (extent is System.Collections.IList list)
            {
                list.Clear();
            }
        }
    }
    
    [Test]
    public void PersonCreatedProperly()
    {
        var initialCount = Person.GetAll().Count;

        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.That(Person.GetAll().Count, Is.EqualTo(initialCount + 1));
        Assert.That(person.FirstName, Is.EqualTo("Vasia"));
        Assert.That(person.LastName, Is.EqualTo("Pupkin"));
        Assert.That(person.PhoneNumber, Is.EqualTo("+123456789"));
    }
    
    [Test]
    public void InvalidFirstNameThrowsArgumentException()
    {
        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.Throws<ArgumentException>(() => person.FirstName = "");
        Assert.Throws<ArgumentException>(() => person.FirstName = null);
        Assert.Throws<ArgumentException>(() => person.FirstName = "A");
        Assert.Throws<ArgumentException>(() => person.FirstName = new string('a', 51));
    }
    
    [Test]
    public void InvalidLastNameThrowsArgumentException()
    {
        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.Throws<ArgumentException>(() => person.LastName = "");
        Assert.Throws<ArgumentException>(() => person.LastName = null);
        Assert.Throws<ArgumentException>(() => person.LastName = "A");
        Assert.Throws<ArgumentException>(() => person.LastName = new string('a', 51));
    }
    
    [Test]
    public void InvalidPhoneNumberThrowsArgumentException()
    {
        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = null);
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "abc123");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "+012345");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "+12345678901234567");
    }

    [Test]
    public void InvalidLegalAdultAgeThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Person.LegalAdultAge = 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => Person.LegalAdultAge = 151);
    }
}