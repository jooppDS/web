using WebStore.Models;
using WebStore.Models.Enums;
using WebStore.Models.ValueObjects;

namespace WebStore.Tests.Inheritance;

[TestFixture]
public class ProductTests
{
    private Seller _seller = null!;

    [SetUp]
    public void Setup()
    {
        _seller = new Seller("fa", new Address("fawtfa", "shneine", "pepe", "1337", "khekhe"));
    }
    
    //weapon
    [Test]
    public void Weapon_ShouldInheritFromProduct()
    {
        var weapon = new Weapon("Pa", "Fawtfapepeshneine", 3500, true, 10, 10,
            TimeSpan.FromDays(365), "2.28mm", 10, 100, _seller);

        Assert.That(weapon, Is.InstanceOf<Product>());
    }

    [Test]
    public void NewWeapon_ShouldHaveTypeNew()
    {
        var weapon = new Weapon("Pa", "Fawtfapepeshneine", 3500, true, 10, 10,
            TimeSpan.FromDays(365), "2.28mm", 10, 100, _seller);
        
            Assert.That(weapon.Type, Is.EqualTo(ProductType.New));
    }

    [Test]
    public void UsedWeapon_AccessingWarranty_ShouldThrow()
    {
        var weapon = new Weapon("Pa", "Fawtfapepeshneine", 3500, true, 10, 10,
            ProductCondition.BattleScarred, "pepepepekhekhe", "2.28mm", 10, 100, _seller);
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = weapon.WarrantyPeriod;
        });
    }
    
    
    //phone
    [Test]
    public void Phone_ShouldInheritFromProduct()
    {
        var phone = new Phone("PEPE", "wtfapepeshneinekhekhe", 3500, false, 2, 20,
            TimeSpan.FromDays(365), true, 500, 5000, "Swagdragon", _seller);
        
        Assert.That(phone, Is.InstanceOf<Product>());
    }

    [Test]
    public void UsedPhone_ShouldHaveUsedType()
    {
        var phone = new Phone("PEPE", "wtfapepeshneinekhekhe", 3500, false, 2, 20,ProductCondition.BattleScarred,
            "FAFA PEPE PEPE", true, 500, 5000, "Swagdragon", _seller);
        
        Assert.That(phone.Type, Is.EqualTo(ProductType.Used));
    }

    [Test]
    public void NewPhone_SettingCondition_ShouldThrow()
    {
        var phone = new Phone("PEPE", "wtfapepeshneinekhekhe", 3500, false, 2, 20,
            TimeSpan.FromDays(365), true, 500, 5000, "Swagdragon", _seller);
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            phone.Condition = ProductCondition.BattleScarred;
        });
    }
    
    //clothing
    [Test]
    public void Clothing_ShouldInheritFromProduct()
    {
        var clothing = new Clothing("FA SHNEINE", "KHEKHEFAWTFA", 3500, false, 100, 15, 
            TimeSpan.FromDays(365), new List<string> { "Cotton", "Polyester" }, 
            ClothingSize.XXL, Gender.Unisex, "wash pepe", _seller);
        
        Assert.That(clothing, Is.InstanceOf<Product>());
    }

    [Test]
    public void UsedClothing_ShouldAllowDefects()
    {
        var clothing = new Clothing("FA SHNEINE", "KHEKHEFAWTFA", 3500, false, 100, 15, 
            ProductCondition.BattleScarred,"FAPEPE", new List<string> { "Cotton", "Polyester" }, 
            ClothingSize.XXL, Gender.Unisex, "wash pepe", _seller);
        
        Assert.That(clothing.DefectsDescription, Is.EqualTo("FAPEPE"));
    }
    
}