using Microsoft.AspNetCore.Mvc;
using SV20T1020020.BusinessLayers;
using SV20T1020020.DomainModels;

namespace SV20T1020020.Web.Controllers
{
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 25;

        public IActionResult Index(int page = 1, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int rowCount = 0;

            var data = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue ?? "", categoryId , supplierId , minPrice , maxPrice );

            var model = new Models.ProductSearchResult()
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
            ViewBag.Title = "Bổ sung Mặt hàng";
            Product model = new Product()
            {
                ProductId = 0,
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
                return Content(ex.Message);
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
                        PhotoId = 0
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
                    ViewBag.Title = "Bổ sung ảnh";
                    return View(model);
                case "edit":
                    ProductAttribute? model1 = ProductDataService.GetAttribute(attributeId);
                    ViewBag.Title = "Thay đổi ảnh";
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
