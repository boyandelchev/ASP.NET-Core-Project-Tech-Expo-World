namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.NewsArticle;

    public class AddNewsFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; init; }

        [Required]
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        [Display(Name = "Select Category")]
        public int NewsCategoryId { get; init; }

        public IEnumerable<NewsCategoryViewModel> NewsCategories { get; set; }
    }
}
