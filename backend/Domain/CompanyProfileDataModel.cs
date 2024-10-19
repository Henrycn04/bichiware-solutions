
namespace backend.Models
{
	public class CompanyProfileDataModel
	{
		public class MembersModel
		{
			public string Username {  get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }
		}
		public class CompanyAddressModel
		{
			public string Province { get; set; }
			public string Canton { get; set; }
			public string District { get; set; }
			public string ExactAddress { get; set; }
		}
		public string CompanyName { get; set; }
		public string Cedula { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public List<CompanyAddressModel> Addresses { get; set; }
		public List<MembersModel> Members { get; set; }
	}
}