using Microsoft.AspNetCore.Mvc;

namespace SV20T1020020.Web.Controllers
{
    public class SupplierController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Nhà cung cấp";
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật thông tin Nhà cung cấp";
            return View();
        }
    }
}
