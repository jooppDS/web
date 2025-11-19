using System.Reflection;
using WebStore.Models;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Tests;

[TestFixture]
public class ReviewTests
{
    private string _testDir = "../../../Data";
    
    [SetUp]
    public void SetUp()
    {
        ClearExtent();
    }

    private static void ClearExtent()
    {
        var type = typeof(Review);
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
    public void ReviewCreatedProperly()
    {
        var initialCount = Review.GetAll().Count;

        var review = new Review(ReviewRating.FiveStars, "Good product");
        Assert.That(Review.GetAll().Count, Is.EqualTo(initialCount + 1));
        Assert.That(review.Rating, Is.EqualTo(ReviewRating.FiveStars));
        Assert.That(review.Comment, Is.EqualTo("Good product"));
    }
    
    [Test]
    public void ReviewCreatedWithoutComment()
    {
        var initialCount = Review.GetAll().Count;

        var review = new Review(ReviewRating.FiveStars);
        Assert.That(Review.GetAll().Count, Is.EqualTo(initialCount + 1));
        Assert.That(review.Rating, Is.EqualTo(ReviewRating.FiveStars));
        Assert.That(review.Comment, Is.Null);
    }
    
    [Test]
    public void InvalidRatingThrowsArgumentOutOfRangeException()
    {
        var review = new Review(ReviewRating.FiveStars);

        Assert.Throws<ArgumentOutOfRangeException>(() => review.Rating = (ReviewRating)999);
    }
    
    [Test]
    public void CommentExceedingMaxLengthThrowsArgumentException()
    {
        var review = new Review(ReviewRating.FiveStars);
        Assert.Throws<ArgumentException>(() => review.Comment = new string('a', 1001));
    }
    
    [Test]
    public void ValidCommentOfMaxLengthAccepted()
    {
        var review = new Review(ReviewRating.FiveStars);
        Assert.DoesNotThrow(() => review.Comment = new string('a', 1000));
        Assert.That(review.Comment!.Length, Is.EqualTo(1000));
    }

    [Test]
    public void NullAndEmptyCommentIsAllowed()
    {
        var review = new Review(ReviewRating.FiveStars);
        Assert.DoesNotThrow(() => review.Comment = null);
        Assert.That(review.Comment, Is.Null);
        Assert.DoesNotThrow(() => review.Comment = "");
        Assert.That(review.Comment, Is.EqualTo(string.Empty));
    }

    [Test]
    public void ExtentCreationSavingAndLoadingWorksCorrectly()
    {
        var initialCount = Review.GetAll().Count;

        var r1 = new Review(ReviewRating.FourStars, "Nice");
        var r2 = new Review(ReviewRating.FiveStars, "Excellent");

        Assert.That(Review.GetAll().Count, Is.EqualTo(initialCount + 2));

        Review.SaveToXml(_testDir);
        Assert.That(XmlPersistenceService.FileExists("Reviews", _testDir), Is.True);

        ClearExtent();

        Assert.That(Review.GetAll().Count, Is.EqualTo(0));

        Review.LoadFromXml(_testDir);

        Assert.That(Review.GetAll().Count, Is.EqualTo(2));
        Assert.That(Review.GetAll()[0].Rating, Is.EqualTo(ReviewRating.FourStars));
        Assert.That(Review.GetAll()[1].Rating, Is.EqualTo(ReviewRating.FiveStars));

        ClearExtent();

        Assert.That(Review.GetAll().Count, Is.EqualTo(0));

        var missingDir = Path.Combine(_testDir, "Missing");
        Review.LoadFromXml(missingDir);

        Assert.That(Review.GetAll().Count, Is.EqualTo(0));
    }
}