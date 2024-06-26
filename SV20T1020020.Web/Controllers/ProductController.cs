﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SV20T1020020.BusinessLayers;
using SV20T1020020.DomainModels;
using SV20T1020020.Web.Models;

namespace SV20T1020020.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}, {WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 25;
        private const string PRODUCT_SEARCH = "product_search"; //Tên biến dùng để lưu trong session

        public IActionResult Index()
        {
            // Kiểm tra xem trong session có lưu điều kiện tìm kiếm không
            // Nếu có thì sử dụng lại điều kiện tìm kiếm, ngược lại thì tìm kiếm theo điều kiện mặc định
            Models.ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryId = 0,
                    SupplierId = 0,
                };
            }

            return View(input);
        }

        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryId, input.SupplierId, 0, 0);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                CategoryID = input.CategoryId,
                SupplierID = input.SupplierId,
                Data = data
            };

            // Lưu lại điều kiện tìm kiếm 
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Mặt hàng";
            Product model = new Product()
            {
                ProductId = 0,
                Photo = "noproduct.png"
            };
            ViewBag.IsEdit = false;//viết tạm
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Mặt hàng";
            Product? model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (String.IsNullOrEmpty(model.Photo))
            {
                model.Photo = "noproduct.png";
            }
            ViewBag.IsEdit = true;//viết tạm
            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            try
            {
                ViewBag.Title = data.ProductId == 0 ? "Bổ sung Mặt hàng" : "Cập nhật thông tin Mặt hàng";
                ViewBag.IsEdit = data.ProductId == 0 ? false : true;
                //Kiểm tra đầu vào và đưa các thông báo lỗi vào trong ModelState (nếu có)
                if (string.IsNullOrWhiteSpace(data.ProductName))
                    ModelState.AddModelError(nameof(data.ProductName), "Tên không được để trống");
                if (data.CategoryId == 0)
                    ModelState.AddModelError(nameof(data.CategoryId), "Vui lòng chọn loại hàng");
                if (data.SupplierId == 0)
                    ModelState.AddModelError(nameof(data.SupplierId), "Vui lòng chọn nhà cung cấp");
                if (string.IsNullOrWhiteSpace(data.Unit))
                    ModelState.AddModelError(nameof(data.Unit), "Đơn vị không được để trống");
                if (string.IsNullOrWhiteSpace(data.Price.ToString()) || data.Price.ToString() == "0")
                    ModelState.AddModelError(nameof(data.Price), "Vui lòng nhập giá mặt hàng");
                if(data.Price < 0)
                    ModelState.AddModelError(nameof(data.Price), "Vui lòng nhập giá mặt hàng lớn hơn 0");
                //Thông qua thuộc tính IsValid của ModelState  để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {
                    return View("Edit", data);
                }
                //Xử lý ảnh upload (nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho product
                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\products");//đường dẫn đến thư mục lưu file
                    string filePath = Path.Combine(folder, fileName); // đường dẫn đến file cần lưu
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                };

                if (data.ProductId == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);
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
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return View("Index");
            }

            ViewBag.AllowDelete = !ProductDataService.IsUsedProduct(id);

            return View(model);
        }
        public IActionResult Photo(int id, string method, long photoId = 0)
        {
            switch(method)
            {
                case "add":
                    ProductPhoto model = new ProductPhoto()
                    {
                        ProductId = id,
                        PhotoId = 0,
                        Photo = "noproduct.png",
                        IsHidden = true
                    };
                    ViewBag.Title = "Bổ sung ảnh";
                    return View(model);
                case "edit":
                    ProductPhoto? model1 = ProductDataService.GetPhoto(photoId);
                    ViewBag.Title = "Thay đổi ảnh";
                    return View(model1);
                case "delete":
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default: 
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            try
            {
                ViewBag.Title = data.PhotoId == 0 ? "Bổ sung ảnh" : "Thay đổi ảnh";
                //Kiểm tra đầu vào và đưa các thông báo lỗi vào trong ModelState (nếu có)
                if (string.IsNullOrWhiteSpace(data.DisplayOrder.ToString()) || data.DisplayOrder.ToString() == "0")
                    ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng nhập vị trí của ảnh");
                if (data.DisplayOrder < 1)
                    ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng nhập vị trí của ảnh lớn hơn 1");
                List<ProductPhoto> listPhotos = ProductDataService.ListPhotos(data.ProductId);
                foreach (ProductPhoto item in listPhotos)
                {
                    if (data.DisplayOrder == item.DisplayOrder && data.PhotoId != item.PhotoId)
                    {
                        ModelState.AddModelError(nameof(data.DisplayOrder), $"Thứ tự hiển thị {data.DisplayOrder} không hợp lệ");
                        break;
                    }
                }
                //Thông qua thuộc tính IsValid của ModelState  để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {
                    return View("Photo", data);
                }

                //Xử lý ảnh upload (nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho product
                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\products");//đường dẫn đến thư mục lưu file
                    string filePath = Path.Combine(folder, fileName); // đường dẫn đến file cần lưu
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                };

                if (data.PhotoId == 0)
                {
                    long id = ProductDataService.AddPhoto(data);
                }
                else
                {
                    bool result = ProductDataService.UpdatePhoto(data);
                }
                return RedirectToAction("Edit", new { id = data.ProductId });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public IActionResult Attribute(int id, string method, long attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ProductAttribute model = new ProductAttribute()
                    {
                        ProductId = id,
                        AttributeId = 0
                    };
                    ViewBag.Title = "Bổ sung Thuộc tính";
                    return View(model);
                case "edit":
                    ProductAttribute? model1 = ProductDataService.GetAttribute(attributeId);
                    ViewBag.Title = "Thay đổi Thuộc tính";
                    return View(model1);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult SaveAttribute(ProductAttribute data)
        {
            try
            {
                ViewBag.Title = data.AttributeId == 0 ? "Bổ sung Thuộc tính" : "Thay đổi Thuộc tính";
                //Kiểm tra đầu vào và đưa các thông báo lỗi vào trong ModelState (nếu có)
                if (string.IsNullOrWhiteSpace(data.AttributeName))
                    ModelState.AddModelError(nameof(data.AttributeName), "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                    ModelState.AddModelError(nameof(data.AttributeValue), "Giá trị không được để trống");
                if (string.IsNullOrWhiteSpace(data.DisplayOrder.ToString()) || data.DisplayOrder.ToString() == "0")
                    ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng nhập vị trí của ảnh"); 
                if (data.DisplayOrder < 1)
                    ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng nhập vị trí của ảnh lớn hơn 1");
                List<ProductAttribute> listAttributes = ProductDataService.ListAttributes(data.ProductId);
                foreach (ProductAttribute item in listAttributes)
                {
                    if (data.DisplayOrder == item.DisplayOrder && data.AttributeId != item.AttributeId)
                    {
                        ModelState.AddModelError(nameof(data.DisplayOrder), $"Thứ tự hiển thị {data.DisplayOrder} không hợp lệ");
                        break;
                    }
                }

                //Thông qua thuộc tính IsValid của ModelState  để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {
                    return View("Attribute", data);
                }
                if (data.AttributeId == 0)
                {
                    long id = ProductDataService.AddAttribute(data);
                }
                else
                {
                    bool result = ProductDataService.UpdateAttribute(data);
                }
                return RedirectToAction("Edit", new { id = data.ProductId });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
