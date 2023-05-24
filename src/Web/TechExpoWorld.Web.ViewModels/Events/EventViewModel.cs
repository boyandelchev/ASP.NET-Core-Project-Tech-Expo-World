namespace TechExpoWorld.Web.ViewModels.Events
{
    public class EventViewModel : IEventModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Location { get; init; }

        public string StartDate { get; init; }

        public string EndDate { get; init; }
    }
}
