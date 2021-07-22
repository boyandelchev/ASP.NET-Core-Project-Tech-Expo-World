namespace TechExpoWorld.Models.News
{
    using System;

    public class AllNewsQueryModel
    {
        public int Id { get; init; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string CreatedOn { get; init; }

        public string NewsCategory { get; set; }

        public string Author { get; init; }
    }
}
