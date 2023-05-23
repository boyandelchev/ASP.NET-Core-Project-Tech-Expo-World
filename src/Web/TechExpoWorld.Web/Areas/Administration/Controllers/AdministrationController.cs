namespace TechExpoWorld.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using TechExpoWorld.Web.Controllers;

    using static TechExpoWorld.Common.GlobalConstants.Admin;

    [Authorize(Roles = RoleName)]
    [Area(AreaName)]
    public class AdministrationController : BaseController
    {
    }
}
