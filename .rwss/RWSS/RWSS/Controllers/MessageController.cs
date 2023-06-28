using Microsoft.AspNetCore.Mvc;

namespace RWSS.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
