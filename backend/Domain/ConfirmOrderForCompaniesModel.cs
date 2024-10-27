using backend.Domain;

namespace backend.Models;

public class ConfirmOrderForCompaniesModel
{
    public int CompanyId { set; get; }
    public string CompanyName { set; get; }
    public string CompanyEmail { set; get; }
    public List<OrderProductModel> OrderProducts { get; set; }
    public double Taxes { get; set; }
    public double ShippingCost { get; set; }
    public double ProductCost { get; set; }
}
