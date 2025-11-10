using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;

namespace WebStore.Models
{
    public class Review
    {
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
    }
}

