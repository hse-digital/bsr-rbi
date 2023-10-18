namespace HSE.RP.API.Models.Payment.Request
{

    public class NewCardPaymentRequest
    {
        public string Reference { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Amount { get; set; }
        public string ReturnUrl { get; set; } = null!;
        public GovukPaymentCardHolderDetails CardHolder { get; set; } = null!;
        public string Environment { get; set; } = null!;
        public string Application { get; set; } = null!;
        public string ApplicationId { get; set; } = null!;
    }

    public class GovukPaymentCardHolderDetails
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public GovukPaymentCardHolderAddress Address { get; set; } = null!;
    }

    public class GovukPaymentCardHolderAddress
    {
        public string Line1 { get; set; } = null!;
        public string Line2 { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
