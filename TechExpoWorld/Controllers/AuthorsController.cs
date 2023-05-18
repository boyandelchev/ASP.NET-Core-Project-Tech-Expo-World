namespace TechExpoWorld.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Infrastructure.Extensions;
    using TechExpoWorld.Models.Authors;
    using TechExpoWorld.Services.Authors;

    using static GlobalConstants.TempData;

    public class AuthorsController : Controller
    {
        private const string ControllerNews = "News";
        private readonly IAuthorService authors;

        public AuthorsController(IAuthorService authors)
            => this.authors = authors;

        [Authorize]
        public async Task<IActionResult> BecomeAuthor()
        {
            if (await this.authors.IsAuthor(this.User.Id()) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BecomeAuthor(BecomeAuthorFormModel author)
        {
            var userId = this.User.Id();

            if (await this.authors.IsAuthor(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            await this.authors.Create(
                author.Name,
                author.PhoneNumber,
                author.Address,
                author.PhotoUrl,
                userId);

            TempData[GlobalMessageKey] = CreatedAuthor;

            return RedirectToAction(nameof(NewsController.All), ControllerNews);
        }
    }
}
