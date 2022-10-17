using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcProject.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
       // [AllowAnonymous] otorize kaldırabiliriz
        public IActionResult Index()
        {
            return View();
        }
    }
}
