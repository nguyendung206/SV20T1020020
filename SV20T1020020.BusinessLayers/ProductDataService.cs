using SV20T1020020.DataLayers;
using SV20T1020020.DataLayers.SQLServer;
using SV20T1020020.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020020.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;

        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectingString);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(string searchValue = "")
        {
            return productDB.List().ToList();
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0,
                                                string searchValue = "", int categoryId = 0, int supplierId = 0,
                                                decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue, categoryId, supplierId, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice).ToList();

        }

        /// <summary>
        /// Lấy thông tin của một mặt hàng theo id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Product? GetProduct(int productId)
        {
            return productDB.Get(productId);
        }

        /// <summary>
        /// Thêm 1 mặt hàng
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }

        /// <summary>
        /// Cập nhật mặt hàng
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }

        /// <summary>
        /// Xóa 1 mặt hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productId)
        {
            if (productDB.IsUsed(productId))
                return false;
            return productDB.Delete(productId);
        }

        /// <summary>
        /// Kiểm tra xem mặt hàng có mã id hiện có dữ liệu liên quan hay không 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedProduct(int productId)
        {
            return productDB.IsUsed(productId);
        }

        public static List<ProductPhoto> ListPhotos(int productId)
        {
            return productDB.ListPhotos(productId).ToList();
        }

        public static ProductPhoto? GetPhoto(long photoId)
        {
            return productDB.GetPhoto(photoId);
        }

        /// <summary>
        /// Thêm 1 ảnh của mặt hàng
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }

        /// <summary>
        /// Cập nhật ảnh mặt hàng
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }

        /// <summary>
        /// Xóa 1 ảnh của mặt hàng có mã là photoId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeletePhoto(long photoId)
        {
            return productDB.DeletePhoto(photoId);
        }

        public static List<ProductAttribute> ListAttributes(int productId)
        {
            return productDB.ListAttributes(productId).ToList();
        }

        public static ProductAttribute? GetAttribute(long attributeId)
        {
            return productDB.GetAttribute(attributeId);
        }

        /// <summary>
        /// Thêm 1 ảnh của mặt hàng
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }

        /// <summary>
        /// Cập nhật ảnh mặt hàng
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }

        /// <summary>
        /// Xóa 1 ảnh của mặt hàng có mã là attributeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteAttribute(long attributeId)
        {
            return productDB.DeleteAttribute(attributeId);
        }

    }
}
