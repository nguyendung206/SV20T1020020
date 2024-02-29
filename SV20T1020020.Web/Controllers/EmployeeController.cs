using Microsoft.AspNetCore.Mvc;
using SV20T1020020.BusinessLayers;
using SV20T1020020.DomainModels;

namespace SV20T1020020.Web.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;

            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.EmployeeSearchResult()
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
            ViewBag.Title = "Bổ sung Nhân viên";
            Employee model = new Employee()
            {
                EmployeeId = 0,
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Nhân viên";
            Employee? model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Employee data)
        {
            try
            {
                if (data.EmployeeId == 0)
                {
                    int id = CommonDataService.AddEmployee(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateEmployee(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return View("Index");
            }

            ViewBag.AllowDelete = !CommonDataService.IsUsedEmployee(id);

            return View(model);
        }
    }
}
