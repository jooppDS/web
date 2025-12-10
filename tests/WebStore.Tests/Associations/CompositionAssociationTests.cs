using System.Reflection;
using WebStore.Models;
using WebStore.Models.ValueObjects;

namespace WebStore.Tests;

[TestFixture]
public class CompositionAssociationTests
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
    public void CreateProduct_ShouldAutomaticallyAddToSeller()
    {
        var seller = new Seller("SellerName", new Address());
        var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), seller);
        var product2 = new New("product2", "description", 10, false, 10, 10, new TimeSpan(1), seller);
        
        Assert.That(seller.Products.Count, Is.EqualTo(2));
        Assert.That(seller.Products.Contains(product1), Is.True);
        Assert.That(seller.Products.Contains(product2), Is.True);
    }
    
    [Test]
    public void CreateProduct_NullSeller_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), null));
    }
    
    [Test]
    public void RemoveProduct_ShouldDeleteProduct()
    {
        var seller = new Seller("SellerName1", new Address());
        var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), seller);

        seller.RemoveProduct(product1);
        Assert.That(seller.Products.Count, Is.EqualTo(0));
        Assert.That(Seller.GetAll().Count, Is.EqualTo(1));
        Assert.That(Product.GetAll().Count, Is.EqualTo(0));
    }
    
    [Test] 
    public void DeleteSeller_ShouldCascadeDeleteProducts()
    {
        var seller = new Seller("SellerName", new Address());
        var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), seller);
        var product2 = new New("product2", "description", 10, false, 10, 10, new TimeSpan(1), seller);
        
        Assert.That(Product.GetAll().Count, Is.EqualTo(2));
        
        seller.Delete();
        
        Assert.That(Seller.GetAll().Count, Is.EqualTo(0));
        Assert.That(Product.GetAll().Count, Is.EqualTo(0));
    }
}