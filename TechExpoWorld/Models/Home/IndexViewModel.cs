namespace TechExpoWorld.Models.Home
{
    using System.Collections.Generic;
    using TechExpoWorld.Services.News.Models;

    public class IndexViewModel
    {
        public int TotalNewsArticles { get; init; }

        public int TotalUsers { get; init; }

        public int TotalAuthors { get; init; }

        public int TotalAttendees { get; init; }

        public int TotalEvents { get; init; }

        public int TotalSpeakers { get; init; }

        public int TotalExhibitors { get; init; }

        public int TotalLocations { get; init; }

        public List<NewsArticleIndexServiceModel> News { get; set; }
    }
}
