using System.Reflection;
using WebStore.Models;
using WebStore.Models.ValueObjects;

namespace WebStore.Tests;

[TestFixture]
public class QualifiedAssociationTests
{
    [SetUp]
    public void SetUp()
    {
        ClearExtent<Seller>();
        ClearExtent<New>();
        ClearExtent<Product>();
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
    public void CreateProduct_SameName_ShouldThrowInvalidOperationException()
    {
        var seller = new Seller("SellerName", new Address());
        new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), seller);

        Assert.Throws<InvalidOperationException>(() =>
            new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), seller));
    }

    [Test]
    public void AddProduct_NullProduct_ShouldThrowInvalidOperationException()
    {
        var seller = new Seller("SellerName", new Address());

        Assert.Throws<ArgumentNullException>(() => seller.AddProduct(null!));
    }
    
    [Test]
    public void AddProduct_SameSeller_IsIdempotent()
    {
        var seller = new Seller("SellerName", new Address());
        var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), seller);

        Assert.DoesNotThrow(() => seller.AddProduct(product1));
        Assert.That(seller.Products.Count, Is.EqualTo(1));
    }

    [Test]
    public void SetSellerInternal_NullSeller_ShouldThrowInvalidOperationException()
    {
        var product1 = 
            new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        
        Assert.Throws<ArgumentNullException>(() => product1.SetSellerInternal(null!));
    }

    [Test]
    public void DeleteProduct_ShouldRemoveProductFromSeller()
    {
        var seller = new Seller("SellerName1", new Address());
        var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), seller);
        
        product1.Delete();
        Assert.That(seller.Products.Count, Is.EqualTo(0));
    }
}