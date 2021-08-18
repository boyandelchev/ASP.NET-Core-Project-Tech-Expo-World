namespace TechExpoWorld.Test.Pipeline
{
    using MyTested.AspNetCore.Mvc;
    using TechExpoWorld.Controllers;
    using TechExpoWorld.Models.Home;
    using Xunit;

    using static Data.NewsArticles;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithData(TenNewsArticles))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IndexViewModel>()
                    .Passing(model =>
                    {
                        Assert.Equal(3, model.News.Count);
                    }));

        [Fact]
        public void ErrorShouldReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();
    }
}
