namespace Tabeeb.Application.Areas.Customer.Queries.GetCustomers
{
    public class CustomerDto
    {
        public long Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;


        public string PhoneNumber { get; set; } = null!;

        public string IdentityNumber { get; set; } = null!;

        public long Medicalfile {  get; set; }
        public string UserId { get; set; } = null!;
    }
}