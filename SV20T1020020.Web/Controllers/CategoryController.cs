using Microsoft.AspNetCore.Mvc;
using SV20T1020020.BusinessLayers;
using SV20T1020020.DomainModels;

namespace SV20T1020020.Web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;

            var data = CommonDataService.ListOfCategories(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.CategorySearchResult()
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
            ViewBag.Title = "Bổ sung Người giao hàng";
            Category model = new Category()
            {
                CategoryId = 0,
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Người giao hàng";
            Category? model = CommonDataService.GetCategory(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Category data)
        {
            try
            {
                if (data.CategoryId == 0)
                {
                    int id = CommonDataService.AddCategory(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateCategory(data);
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
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCategory(id);
            if (model == null)
            {
                return View("Index");
            }

            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);

            return View(model);
        }
    }
}
