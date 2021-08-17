namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Authors;
    using TechExpoWorld.Services.Authors;

    using static WebConstants;

    public class AuthorsController : Controller
    {
        private readonly IAuthorService authors;

        public AuthorsController(IAuthorService authors)
            => this.authors = authors;

        [Authorize]
        public IActionResult BecomeAuthor()
        {
            var userId = this.User.Id();

            if (this.authors.IsAuthor(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult BecomeAuthor(BecomeAuthorFormModel author)
        {
            var userId = this.User.Id();

            if (this.authors.IsAuthor(userId) || this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            this.authors.Create(
                author.Name,
                author.PhoneNumber,
                author.Address,
                author.PhotoUrl,
                userId);

            TempData[GlobalMessageKey] = "Thank you for becomming an author!";

            return RedirectToAction(nameof(NewsController.All), "News");
        }
    }
}
