namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Authors;
    using TechExpoWorld.Services.Authors;

    public class AuthorsController : Controller
    {
        private readonly IAuthorService authors;

        public AuthorsController(IAuthorService authors)
            => this.authors = authors;

        [Authorize]
        public IActionResult BecomeAuthor() => View();

        [HttpPost]
        [Authorize]
        public IActionResult BecomeAuthor(BecomeAuthorFormModel author)
        {
            var userId = this.User.Id();

            if (this.authors.IsAuthor(userId))
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

            return RedirectToAction(nameof(NewsController.All), "News");
        }
    }
}
