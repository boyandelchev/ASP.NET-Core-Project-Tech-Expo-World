﻿namespace TechExpoWorld.Services.News
{
    public class NewsArticleServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Content { get; init; }

        public string ImageUrl { get; init; }

        public string CreatedOn { get; init; }

        public string CategoryName { get; init; }

        public string AuthorName { get; init; }
    }
}
