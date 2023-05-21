namespace TechExpoWorld.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    using TechExpoWorld.Controllers;

    public static class EndpointRouteBuilderExtensions
    {
        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        public static void MapNewsArticleDetailsControllerRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute(
                name: "News Article Details",
                pattern: "/News/Details/{id}/{information}",
                defaults: new
                {
                    controller = typeof(NewsController).GetControllerName(),
                    action = nameof(NewsController.Details)
                });

        public static void MapEventDetailsControllerRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute(
                name: "Event Details",
                pattern: "/Events/Details/{id}/{information}",
                defaults: new
                {
                    controller = typeof(EventsController).GetControllerName(),
                    action = nameof(EventsController.Details)
                });

        public static void MapCancelTicketsControllerRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute(
                name: "Cancel Tickets",
                pattern: "{controller=Home}/{action=Index}/{id}/{ticketId}");
    }
}
