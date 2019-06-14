using DotNetCoreAssessment.Models;

namespace DotNetCoreAssessment.Web.ViewModels
{
    public class AddOrderItemViewModel
    {
        public Order Order { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
