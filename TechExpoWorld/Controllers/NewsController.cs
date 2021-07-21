namespace TechExpoWorld.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TechExpoWorld.Data;

    public class NewsController : Controller
    {
        private readonly TechExpoDbContext data;

        public NewsController(TechExpoDbContext data)
            => this.data = data;

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult All()
        {
            return View();
        }
    }
}
