namespace TechExpoWorld.Web.ViewModels.News
{
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; init; }

        public string Name { get; init; }
    }
}
