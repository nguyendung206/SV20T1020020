using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020020.BusinessLayers
{
    /// <summary>
    /// Khởi tạo, lưu trữ các thông tin cấu hình của BusinessLayer
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Chuỗi kết thông số kết nối đến CSDL
        /// </summary>
        public static string ConnectiongString { get; private set; } = "";

        /// <summary>
        /// Khởi tạo cấu hình cho BussinessLayer
        /// (Hàm này phải được gọi trước khi ứng dụng chạy)
        /// </summary>
        /// <param name="connectiongString"></param>
        public static void Initialize(string connectiongString)
        {
            Configuration.ConnectiongString = connectiongString;
        }
    }
}
