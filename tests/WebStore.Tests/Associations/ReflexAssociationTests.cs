using System.Reflection;
using System.Runtime.CompilerServices;
using WebStore.Models;
using WebStore.Models.Enums;

namespace WebStore.Tests;
[TestFixture]
public class ReflexAssociationTests
{
    [SetUp]
    public void Setup()
    {
        ClearExtent<Clothing>();
    }

    private static void ClearExtent<T>()
    {
        var type = typeof(T);
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
    public void AddRelatedClothing_CreatesBidirectionalLink()
    {

        var clothing1 = new Clothing("Cherevichki", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        var clothing2 = new Clothing("Kashaloty", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        
        clothing1.AddRelatedClothing(clothing2);
        
        Assert.That(clothing1.RelatedClothing.Contains(clothing2), Is.True);
        Assert.That(clothing2.RelatedClothing.Contains(clothing1), Is.True);
    }

    [Test]
    public void RemoveRelatedClothing_RemovesBidirectionalLink()
    {
        var clothing1 = new Clothing("Cherevichki", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        var clothing2 = new Clothing("Kashaloty", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        
        clothing1.AddRelatedClothing(clothing2);
        clothing1.RemoveRelatedClothing(clothing2);
        Assert.That(clothing1.RelatedClothing.Contains(clothing2), Is.False);
        Assert.That(clothing2.RelatedClothing.Contains(clothing1), Is.False);
    }

    [Test]
    public void Delete_RemovesClothingFromAllRelatedItems()
    {
        var clothing1 = new Clothing("Cherevichki", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        var clothing2 = new Clothing("Kashaloty", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        var clothing3 = new Clothing("Skorohody", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        
        clothing1.AddRelatedClothing(clothing2);
        clothing1.AddRelatedClothing(clothing3);
        
        clothing1.Delete();
        
        Assert.That(clothing2.RelatedClothing.Contains(clothing1), Is.False);
        Assert.That(clothing3.RelatedClothing.Contains(clothing1), Is.False);
        
        Assert.That(Clothing.GetAll().Contains(clothing1), Is.False);
    }

    [Test]
    public void AddRelatedCloting_DoesNotLink_ToItself()
    {
        var clothing1 = new Clothing("Cherevichki", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        clothing1.AddRelatedClothing(clothing1);
        Assert.That(clothing1.RelatedClothing.Contains(clothing1), Is.False);
    }

    [Test]
    public void AddRelatedClothing_DoesNotAllow_Duplicates()
    {
        var clothing1 = new Clothing("Cherevichki", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        var clothing2 = new Clothing("Kashaloty", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        
        clothing1.AddRelatedClothing(clothing2);
        clothing1.AddRelatedClothing(clothing2);
        
        Assert.That(clothing1.RelatedClothing.Count, Is.EqualTo(1));
        Assert.That(clothing2.RelatedClothing.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void RemoveRelatedClothing_RemovesReverseLink()
    {
        var clothing1 = new Clothing("Cherevichki", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());
        var clothing2 = new Clothing("Kashaloty", "zayebatyye", 1337, false, 100, 5,
            new List<string> { "Kozha bobra", "stru4ok bobrovy" }, ClothingSize.XXL, Gender.Unisex, "Da poh",
            new Seller());

        clothing1.AddRelatedClothing(clothing2);

        clothing2.RemoveRelatedClothing(clothing1);

        Assert.That(clothing1.RelatedClothing.Contains(clothing2), Is.False);
        Assert.That(clothing2.RelatedClothing.Contains(clothing1), Is.False);
    }
    
}