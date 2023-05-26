namespace TechExpoWorld.Web.ViewModels.News
{
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class TagViewModel : IMapFrom<Tag>
    {
        public int Id { get; init; }

        public string Name { get; init; }
    }
}
