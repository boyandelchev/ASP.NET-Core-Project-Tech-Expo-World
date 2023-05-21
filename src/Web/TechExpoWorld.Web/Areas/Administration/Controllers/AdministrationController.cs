namespace TechExpoWorld.Web.Areas.Administration.Controllers
{
    using TechExpoWorld.Common;
    using TechExpoWorld.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
