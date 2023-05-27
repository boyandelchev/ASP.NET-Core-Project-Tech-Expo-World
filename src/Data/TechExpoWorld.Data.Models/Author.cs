namespace TechExpoWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TechExpoWorld.Data.Common.Models;

    using static TechExpoWorld.Common.GlobalConstants.Author;

    public class Author : BaseDeletableModel<string>
    {
        public Author()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; init; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; init; }

        [Required]
        public string PhotoUrl { get; init; }

        [Required]
        public string UserId { get; init; }

        public IEnumerable<NewsArticle> NewsArticles { get; init; } = new List<NewsArticle>();
    }
}
