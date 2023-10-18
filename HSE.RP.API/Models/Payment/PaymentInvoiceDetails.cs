namespace HSE.RP.API.Models.Payment
{
    public class PaymentInvoiceDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string OrderNumberOption { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public int Amount { get; set; }
        public string CreatedDate { get; set; }
        public string Reference { get; set; }

    }
}
