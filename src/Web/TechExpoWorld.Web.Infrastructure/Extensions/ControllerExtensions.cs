namespace TechExpoWorld.Web.Infrastructure.Extensions
{
    using System;

    public static class ControllerExtensions
    {
        private const string Controller = "Controller";

        public static string GetControllerName(this Type controllerType)
            => controllerType.Name.Replace(Controller, string.Empty);
    }
}
