namespace TechExpoWorld.Test.Pipeline
{
    using System.Linq;
    using MyTested.AspNetCore.Mvc;
    using TechExpoWorld.Controllers;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Models.Authors;
    using TechExpoWorld.Models.News;
    using Xunit;

    using static GlobalConstants.TempData;

    public class AuthorsControllerTest
    {
        [Fact]
        public void GetBecomeAuthorShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Authors/BecomeAuthor")
                    .WithUser())
                .To<AuthorsController>(c => c.BecomeAuthor())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Author", "+359888888888", "10 Oxford St London UK", "https://photo.jpg")]
        public void PostBecomeAuthorShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string authorName,
            string phoneNumber,
            string address,
            string photoUrl)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Authors/BecomeAuthor")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new
                    {
                        Name = authorName,
                        PhoneNumber = phoneNumber,
                        Address = address,
                        PhotoUrl = photoUrl
                    })
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<AuthorsController>(c => c.BecomeAuthor(new BecomeAuthorFormModel
                {
                    Name = authorName,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    PhotoUrl = photoUrl
                }))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Author>(authors => authors
                        .Any(a =>
                            a.Name == authorName &&
                            a.PhoneNumber == phoneNumber &&
                            a.Address == address &&
                            a.PhotoUrl == photoUrl &&
                            a.UserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<NewsController>(c => c.All(With.Any<AllNewsQueryModel>())));
    }
}
