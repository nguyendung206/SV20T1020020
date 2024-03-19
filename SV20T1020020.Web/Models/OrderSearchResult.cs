using SV20T1020020.DomainModels;

namespace SV20T1020020.Web.Models
{
    public class OrderSearchResult : BasePaginationResutlt
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
