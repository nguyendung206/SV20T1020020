using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020020.BusinessLayers;
using SV20T1020020.DomainModels;
using SV20T1020020.Web.Models;

namespace SV20T1020020.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}, {WebUserRoles.Employee}")]
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SHIPPER_SEARCH = "shipper_search"; //Tên biến dùng để lưu trong session

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            //Lấy đầu vào tìm kiếm hiện đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH);
            //Trường hợp trong session chưa có điều kiền thì tạo điều kiện mới
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }


            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm session
            ApplicationContext.SetSessionData(SHIPPER_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Người giao hàng";
            Shipper model = new Shipper()
            {
                ShipperId = 0,
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Người giao hàng";
            Shipper? model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Shipper data)
        {
            try
            {
                ViewBag.Title = data.ShipperId == 0 ? "Bổ sung Người giao hàng" : "Cập nhật thông tin Người giao hàng";
                if (string.IsNullOrWhiteSpace(data.ShipperName))
                    ModelState.AddModelError(nameof(data.ShipperName), "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được để trống");
                if (!ModelState.IsValid)
                {
                    return View("Edit", data);
                }
                if (data.ShipperId == 0)
                {
                    int id = CommonDataService.AddShipper(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateShipper(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu. Vui lòng thử lại sau vài phút");
                return View("Edit", data);
            }
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return View("Index");
            }

            ViewBag.AllowDelete = !CommonDataService.IsUsedShipper(id);

            return View(model);
        }
    }
}
