namespace TechExpoWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Services.Data.Authors;
    using TechExpoWorld.Web.Infrastructure.Extensions;
    using TechExpoWorld.Web.ViewModels.Authors;

    using static TechExpoWorld.Common.GlobalConstants.TempData;

    public class AuthorsController : BaseController
    {
        private const string ControllerNews = "News";
        private readonly IAuthorsService authorsService;

        public AuthorsController(IAuthorsService authorsService)
            => this.authorsService = authorsService;

        [Authorize]
        public async Task<IActionResult> BecomeAuthor()
        {
            if (await this.authorsService.IsAuthorAsync(this.User.Id()) || this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BecomeAuthor(BecomeAuthorInputModel input)
        {
            var userId = this.User.Id();

            if (await this.authorsService.IsAuthorAsync(userId) || this.User.IsAdmin())
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.authorsService.CreateAsync(
                input.Name,
                input.PhoneNumber,
                input.Address,
                input.PhotoUrl,
                userId);

            this.TempData[GlobalMessageKey] = CreatedAuthor;

            return this.RedirectToAction(nameof(NewsController.All), ControllerNews);
        }
    }
}
