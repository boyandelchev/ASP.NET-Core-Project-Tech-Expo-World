﻿namespace TechExpoWorld.Models.News
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TechExpoWorld.Services.News;

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

        [Display(Name = "Select Tags")]
        public IEnumerable<int> TagIds { get; init; } = new List<int>();

        public IEnumerable<CategoryServiceModel> Categories { get; set; }

        public IEnumerable<TagServiceModel> Tags { get; set; }
    }
}
