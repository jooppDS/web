using WebStore.Models.ValueObjects;

namespace WebStore.Tests;

public class AddressTests
{
    [Test]
    public void AddressCreatedProperly()
    {
        var initialCount = Address.GetAll().Count;
        
        var address = new Address("Solidarnosci", "Warsaw", "California", "12351", "United States");
        Assert.That(Address.GetAll().Count, Is.EqualTo(initialCount + 1));
        Assert.That(address.Street, Is.EqualTo("Solidarnosci"));
        Assert.That(address.City, Is.EqualTo("Warsaw"));
        Assert.That(address.State, Is.EqualTo("California"));
        Assert.That(address.PostalCode, Is.EqualTo("12351"));
        Assert.That(address.Country, Is.EqualTo("United States"));
    }

    [Test]
    public void InvalidValueThrowsArgumentException()
    {
        var address = new Address("Solidarnosci", "Warsaw", "California", "12351", "United States");
        Assert.Throws<ArgumentException>(() => address.Street = "");
        Assert.Throws<ArgumentException>(() => address.Street = null);
        Assert.Throws<ArgumentException>(() => address.Street = new string('a', 123));
    }

    [Test]
    public void InvalidCityThrowsArgumentException()
    {
        var address = new Address("Solidarnosci", "Warsaw", "California", "12351", "United States");
        Assert.Throws<ArgumentException>(() => address.City = "");
        Assert.Throws<ArgumentException>(() => address.City = null);
        Assert.Throws<ArgumentException>(() => address.City = new string('a', 51));
    }

    [Test]
    public void InvalidStateThrowsArgumentException()
    {
        var address = new Address("Solidarnosci", "Warsaw", "California", "12351", "United States");
        Assert.Throws<ArgumentException>(() => address.State = null);
        Assert.Throws<ArgumentException>(() => address.State = "");
        Assert.Throws<ArgumentException>(() => address.State = new string('a', 51));
    }

    [Test]
    public void InvalidPostalCodeValueThrowsArgumentException()
    {
        var address = new Address("Solidarnosci", "Warsaw", "California", "12351", "United States");
        Assert.Throws<ArgumentException>(() => address.PostalCode = "");
        Assert.Throws<ArgumentException>(() => address.PostalCode = null);
        Assert.Throws<ArgumentException>(() => address.PostalCode = new string('A', 21));
    }

    [Test]
    public void InvalidPostalCodeFormatThrowsArgumentException()
    {
        var address = new Address("Solidarnosci", "Warsaw", "California", "12351", "United States");
        Assert.Throws<ArgumentException>(() => address.PostalCode = "123@123z");
    }

    [Test]
    public void InvalidCountryValueThrowsArgumentException()
    {
        var address = new Address("Solidarnosci", "Warsaw", "California", "12351", "United States");
        Assert.Throws<ArgumentException>(() => address.Country = null);
        Assert.Throws<ArgumentException>(() => address.Country = "");
        Assert.Throws<ArgumentException>(() => address.Country = new string('a', 51));
    }
}