using System.Reflection;
using WebStore.Models;

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
}