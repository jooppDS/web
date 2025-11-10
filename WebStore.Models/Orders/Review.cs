using WebStore.Models.Enums;

namespace WebStore.Models
{
    public class Review
    {
        public ReviewRating Rating { get; set; }
        public string? Comment { get; set; }
    }
}

