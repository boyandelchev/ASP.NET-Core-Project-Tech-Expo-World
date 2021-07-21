namespace TechExpoWorld.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Data;
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Infrastructure;
    using TechExpoWorld.Models.Authors;

    public class AuthorsController : Controller
    {
        private readonly TechExpoDbContext data;

        public AuthorsController(TechExpoDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult BecomeAuthor() => View();

        [HttpPost]
        [Authorize]
        public IActionResult BecomeAuthor(BecomeAuthorFormModel author)
        {
            var userId = this.User.GetId();

            var userIsAlreadyAuthor = this.data
                .Authors
                .Any(d => d.UserId == userId);

            if (userIsAlreadyAuthor)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            var authorData = new Author
            {
                Name = author.Name,
                PhoneNumber = author.PhoneNumber,
                Address = author.Address,
                PhotoUrl = author.PhotoUrl,
                UserId = userId
            };

            this.data.Authors.Add(authorData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(NewsController.All), "News");
        }
    }
}
