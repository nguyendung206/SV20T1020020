using Microsoft.AspNetCore.Mvc;

namespace SV20T1020020.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
