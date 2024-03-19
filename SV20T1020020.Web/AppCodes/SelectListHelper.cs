using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020020.BusinessLayers;

namespace SV20T1020020.Web
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Provinces()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn tỉnh/thành --"
            }); 
            foreach (var item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName
                });
            }

            return list;
        }
        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Tất cả loại hàng --"
            });
            foreach (var item in CommonDataService.ListOfCategory())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryId.ToString(),
                    Text = item.CategoryName
                });
            }
            return list;
        }

        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Tất cả nhà cung cấp --",
            });
            foreach (var item in CommonDataService.ListOfSupplier())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierId.ToString(),
                    Text = item.SupplierName
                });
            }
            return list;
        }
    }
}
