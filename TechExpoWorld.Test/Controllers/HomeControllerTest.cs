namespace TechExpoWorld.Test.Controllers
{
    using System;
    using MyTested.AspNetCore.Mvc;
    using TechExpoWorld.Controllers;
    using TechExpoWorld.Models.Home;
    using Xunit;

    using static Data.NewsArticles;
    using static WebConstants.Cache;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyController<HomeController>
                .Instance(controller => controller
                    .WithData(TenNewsArticles))
                .Calling(c => c.Index())
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(LatestStatisticsAndNewsArticlesCacheKey)
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(5))
                        .WithValueOfType<IndexViewModel>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IndexViewModel>()
                    .Passing(model =>
                    {
                        Assert.Equal(3, model.News.Count);
                    }));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
    }
}
