using SV20T1020020.DomainModels;
namespace SV20T1020020.Web.Models
{

    /// <summary>
    /// Lớp cha cho các lớp biểu diễn dữ liệu kết quả tìm kiếm, phân trang
    /// </summary>
    public abstract class BasePaginationResutlt
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    c += 1;
                }
                return c;
            }
        }

        
    }
    /// <summary>
    /// Kết quả tìm kiếm và lấy danh sách khách hàng
    /// </summary>
    public class CustomerSearchResult : BasePaginationResutlt
    {
        public List<Customer> Data { get; set; }
    }
    public class SupplierSearchResult : BasePaginationResutlt
    {
        public List<Supplier> Data { get; set; }
    }
    public class ShipperSearchResult : BasePaginationResutlt
    {
        public List<Shipper> Data { get; set; }
    }
    public class CategorySearchResult : BasePaginationResutlt
    {
        public List<Category> Data { get; set; }
    }
    public class EmployeeSearchResult : BasePaginationResutlt
    {
        public List<Employee> Data { get; set; }
    }
    public class ProductSearchResult : BasePaginationResutlt
    {
        public List<Product> Data { get; set; }
    }
}
