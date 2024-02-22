using Microsoft.AspNetCore.Mvc;
using SV20T1020020.BusinessLayers;
using SV20T1020020.DomainModels;

namespace SV20T1020020.Web.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 20;
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(out rowCount, page, pageSize, searchValue);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.RowCount = rowCount;

            int pageCount = rowCount / pageSize;
            if (rowCount %  pageSize > 0 )
            {
                pageCount += 1;
            }
            ViewBag.PageCount = pageCount;

            return View(data);
        }

        public IActionResult Create() 
        {
            ViewBag.Title = "Bổ sung Khách hàng";
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật thông tin Khách hàng";
            return View();
        }

        public IActionResult Delete(string id)
        {
            return View();
        }
    }
}
