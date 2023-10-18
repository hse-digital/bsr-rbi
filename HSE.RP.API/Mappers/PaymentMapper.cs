using HSE.RP.API.Models.Payment;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace HSE.RP.API.Mappers
{
    public interface IPaymentMapper
    {
        DynamicsPayment ToDynamics(PaymentResponseModel payment);
        PaymentResponseModel ToPaymentModel(DynamicsPayment dynamicsPayment);
        DynamicsPayment ToDynamics(string buildingApplicationId, DynamicsContact invoicedContact, NewInvoicePaymentRequestModel invoiceData);
        DynamicsPayment ToDynamics(string dynamicsPaymentId, InvoiceData invoiceData);
    }
    public class PaymentMapper : IPaymentMapper
    {
        private readonly IntegrationsOptions integrationOptions;

        public PaymentMapper(IOptions<IntegrationsOptions> integrationOptions)
        {
            this.integrationOptions = integrationOptions.Value;
        }


        public DynamicsPayment ToDynamics(PaymentResponseModel payment)
        {
            return new DynamicsPayment
            {
                bsr_paymentid = payment.bsr_paymentId != null ? payment.bsr_paymentId : null,
                buildingProfessionApplicationReferenceId = payment.ApplicationId != null ? $"/bsr_buildingprofessionapplications({payment.ApplicationId})" : null,
                bsr_lastfourdigitsofcardnumber = payment.LastFourDigitsCardNumber,
                bsr_timeanddateoftransaction = payment.CreatedDate,// <<<>>>
                bsr_transactionid = payment.Reference,
                bsr_paymenttypecode = 760_810_000, // Card
                bsr_service = "RBI",
                bsr_cardexpirydate = payment.CardExpiryDate,
                bsr_billingaddress = string.Join(", ", new[] { payment.AddressLineOne, payment.AddressLineTwo, payment.Postcode, payment.City, payment.Country }.Where(x => !string.IsNullOrWhiteSpace(x))),
                bsr_cardbrandegvisa = payment.CardBrand,
                bsr_cardtypecreditdebit = payment.CardType == "debit" ? DynamicsPaymentCardType.Debit : DynamicsPaymentCardType.Credit,
                bsr_amountpaid = Math.Round((float)payment.Amount / 100, 2),
                bsr_govukpaystatus = payment.Status,
                bsr_govukpaymentid = payment.PaymentId,
                bsr_paymentreconciliationstatus = payment.Status == "success" ? DynamicsPaymentReconciliationStatus.Successful : payment.Status == "failed" ? DynamicsPaymentReconciliationStatus.FailedPayment : DynamicsPaymentReconciliationStatus.Pending
            };
        }

        public PaymentResponseModel ToPaymentModel(DynamicsPayment dynamicsPayment)
        {
            var addressLength = dynamicsPayment.bsr_billingaddress.Length;
            return new PaymentResponseModel
            {
                PaymentId = dynamicsPayment.bsr_paymentid,
                LastFourDigitsCardNumber = dynamicsPayment.bsr_lastfourdigitsofcardnumber,
                CreatedDate = dynamicsPayment.bsr_timeanddateoftransaction.ToString(),
                Reference = dynamicsPayment.bsr_transactionid,
                CardExpiryDate = dynamicsPayment.bsr_cardexpirydate,
                AddressLineOne = dynamicsPayment.bsr_billingaddress.Split(',')[0],
                Postcode = dynamicsPayment.bsr_billingaddress.Split(',')[addressLength - 3],
                City = dynamicsPayment.bsr_billingaddress.Split(',')[addressLength - 2],
                Country = dynamicsPayment.bsr_billingaddress.Split(',')[addressLength - 1],
                CardBrand = dynamicsPayment.bsr_cardbrandegvisa,
                CardType = dynamicsPayment.bsr_cardtypecreditdebit == DynamicsPaymentCardType.Debit ? "debit" : "credit",
                Amount = Convert.ToInt32(dynamicsPayment.bsr_amountpaid) * 100,
                Status = dynamicsPayment.bsr_govukpaystatus,
            };
        }


        public DynamicsPayment ToDynamics(string buildingApplicationId, DynamicsContact invoicedContact, NewInvoicePaymentRequestModel invoiceData)
        {
            return new DynamicsPayment
            {
                bsr_invoicedcontactid = $"/contacts({invoicedContact.contactid})",
                buildingProfessionApplicationReferenceId = $"/bsr_buildingprofessionapplications({buildingApplicationId})",
                bsr_paymenttypecode = 760_810_001, // Invoice
                bsr_service = "RBI",
                bsr_billingaddress = string.Join(", ", new[] { invoiceData.AddressLine1, invoiceData.AddressLine2, invoiceData.Postcode, invoiceData.Town }.Where(x => !string.IsNullOrWhiteSpace(x))),
                bsr_amountpaid = Math.Round(integrationOptions.PaymentAmount / 100, 2),
                bsr_purchaseordernumberifsupplied = invoiceData.OrderNumber,
                bsr_govukpaystatus = "open",
                bsr_emailaddress = invoiceData.Email
            };
        }

        public DynamicsPayment ToDynamics(string dynamicsPaymentId, InvoiceData invoiceData)
        {
            return new DynamicsPayment
            {
                bsr_paymentid = dynamicsPaymentId,
                bsr_invoicenumber = invoiceData.InvoiceNumber,
                bsr_invoicecreationdate = UnixTimeStampToDateTime(invoiceData.CreatedDate).ToString(CultureInfo.InvariantCulture),
                bsr_transactionid = invoiceData.InvoiceId,
                bsr_govukpaystatus = invoiceData.Status
            };
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
