namespace TechExpoWorld.Web.ViewModels.Attendees
{
    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    public class CountryViewModel : AttendeeWorkDetailsModel, IMapFrom<Country>
    {
    }
}
