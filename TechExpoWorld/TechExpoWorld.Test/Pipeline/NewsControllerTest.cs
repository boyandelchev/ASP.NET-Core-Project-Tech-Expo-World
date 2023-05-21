namespace TechExpoWorld.Test.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;

    using MyTested.AspNetCore.Mvc;

    using TechExpoWorld.Controllers;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.News;
    using TechExpoWorld.Services.News.Models;

    using Xunit;

    using static Data.NewsArticles;
    using static GlobalConstants.TempData;

    public class NewsControllerTest
    {
        [Fact]
        public void AllShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap("/News/All")
                .To<NewsController>(c => c.All(new AllNewsQueryModel()))
                .Which(controller => controller
                    .WithData(TenNewsArticles))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllNewsQueryModel>());

        [Fact]
        public void DetailsShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap("/News/Details/1/Article")
                .To<NewsController>(c => c.Details(1, "Article"))
                .Which(controller => controller
                    .WithData(OneNewsArticle))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<NewsArticleWithCommentsViewModel>()
                    .Passing(model =>
                    {
                        Assert.Equal("Article", model.NewsArticle.Title);
                        Assert.Equal("content", model.NewsArticle.Content);
                    }));

        [Fact]
        public void MyNewsArticlesShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/News/MyNewsArticles")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<NewsController>(c => c.MyNewsArticles())
                .Which(controller => controller
                    .WithData(TenNewsArticles))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<NewsArticleServiceModel>>());

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/News/Add")
                    .WithUser())
                .To<NewsController>(c => c.Add())
                .Which(controller => controller
                    .WithData(new Author { UserId = TestUser.Identifier }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("AI", "AI and Big Data", "https://photo.jpg", 0)]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string title,
            string content,
            string imageUrl,
            int categoryId)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/News/Add")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new
                    {
                        Title = title,
                        Content = content,
                        ImageUrl = imageUrl,
                        CategoryId = categoryId
                    })
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<NewsController>(c => c.Add(new NewsArticleFormModel
                {
                    Title = title,
                    Content = content,
                    ImageUrl = imageUrl,
                    CategoryId = categoryId
                }))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                //.Data(data => data
                //    .WithSet<NewsArticle>(newsArticle => newsArticle
                //        .Any(a =>
                //            a.Title == title &&
                //            a.Content == content &&
                //            a.ImageUrl == imageUrl &&
                //            a.NewsCategoryId == categoryId &&
                //            a.NewsArticleTags.Select(t => t.TagId) == tagIds &&
                //            a.Author.UserId == TestUser.Identifier)))
                //.TempData(tempData => tempData
                //    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AuthorsController>(c => c.BecomeAuthor()));

        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnRedirectForNonAuthor()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/News/Edit/1")
                    .WithUser())
                .To<NewsController>(c => c.Edit(1))
                .Which(controller => controller
                    .WithData(OneNewsArticle))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AuthorsController>(c => c.BecomeAuthor()));
    }
}
