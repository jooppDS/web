using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Review
    {
        private static List<Review> _extent = new List<Review>();

        private ReviewRating _rating;
        private string? _comment;

        [Required(ErrorMessage = "Rating is required")]
        public ReviewRating Rating
        {
            get => _rating;
            set
            {
                if (!Enum.IsDefined(typeof(ReviewRating), value))
                    throw new ArgumentOutOfRangeException(nameof(Rating), 
                        "Rating must be a valid ReviewRating value");
                _rating = value;
            }
        }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string? Comment
        {
            get => _comment;
            set
            {
                if (value != null && value.Length > 1000)
                    throw new ArgumentException("Comment cannot exceed 1000 characters", nameof(Comment));
                _comment = value;
            }
        }

        public static List<Review> GetAll()
        {
            return new List<Review>(_extent);
        }

        
        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Reviews", directory);
        }

        
        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Reviews", directory))
                return;

            var loadedReviews = XmlPersistenceService.LoadFromXml<Review>("Reviews", directory);
            
            
            _extent.Clear();
            foreach (var review in loadedReviews)
            {
                _extent.Add(review);
            }
        }

        
        public Review()
        {
            
        }

        public Review(ReviewRating rating, string? comment = null)
        {
            Rating = rating;
            Comment = comment;
            _extent.Add(this);
        }
    }
}

