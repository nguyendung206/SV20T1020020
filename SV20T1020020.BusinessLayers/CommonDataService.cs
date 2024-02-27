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
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu chung
    /// (tỉnh/thành, khách hàng, nhà cung cấp, loại hàng, người giao hàng, nhân viên)
    /// </summary>
    public class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> customerDB;

        /// <summary>
        /// Ctor(Câu hỏi: static contructor hoạt động như thế nào?)
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectiongString;
            provinceDB = new ProvinceDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
        }

        /// <summary>
        /// Danh sách các tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        
        /// <summary>
        /// Lấy thông tin của một khách hàng theo id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }

        /// <summary>
        /// Thêm 1 khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer customer)
        {
            return customerDB.Add(customer);
        }

        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer customer)
        {
            return customerDB.Update(customer);
        }

        /// <summary>
        /// Xóa 1 khách hàng có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            if (customerDB.IsUsed(id))
                return false;
            return customerDB.Delete(id);
        }

        /// <summary>
        /// Kiểm tra xem khách hàng có mã id hiện có dữ liệu liên quan hay không 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id)
        {
            return customerDB.IsUsed(id);
        }
    }
}
