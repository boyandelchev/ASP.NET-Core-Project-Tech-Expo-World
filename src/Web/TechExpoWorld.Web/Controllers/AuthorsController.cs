namespace TechExpoWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Services.Data.Authors;
    using TechExpoWorld.Web.Infrastructure.Extensions;
    using TechExpoWorld.Web.ViewModels.Authors;

    using static TechExpoWorld.Common.GlobalConstants.TempData;

    public class AuthorsController : Controller
    {
        private const string ControllerNews = "News";
        private readonly IAuthorsService authors;

        public AuthorsController(IAuthorsService authors)
            => this.authors = authors;

        [Authorize]
        public async Task<IActionResult> BecomeAuthor()
        {
            if (await this.authors.IsAuthorAsync(this.User.Id()) || this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BecomeAuthor(BecomeAuthorInputModel author)
        {
            var userId = this.User.Id();

            if (await this.authors.IsAuthorAsync(userId) || this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(author);
            }

            await this.authors.CreateAsync(
                author.Name,
                author.PhoneNumber,
                author.Address,
                author.PhotoUrl,
                userId);

            this.TempData[GlobalMessageKey] = CreatedAuthor;

            return this.RedirectToAction(nameof(NewsController.All), ControllerNews);
        }
    }
}
