using Microsoft.AspNetCore.Mvc;
using SV20T1020020.BusinessLayers;
using SV20T1020020.DomainModels;

namespace SV20T1020020.Web.Controllers
{
    public class CustomerController : Controller
    {
        private const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
           
            var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            
            var model = new Models.CustomerSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Khách hàng";
            Customer model = new Customer()
            {
                CustomerId = 0,
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Khách hàng";
            Customer? model = CommonDataService.GetCustomer(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
