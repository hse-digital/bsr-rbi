using Flurl;
using Flurl.Http;
using HSE.RP.API.Mappers;
using HSE.RP.API.Models;
using HSE.RP.API.Models.Payment;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HSE.RP.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseModel> CreateCardPayment(NewCardPaymentRequest newCardPaymentRequest);
        Task<PaymentResponseModel> GetPaymentStatus(string referenceNumber);
        NewCardPaymentRequest BuildPaymentRequest(BuildingProfessionApplicationModel model);
        Task NewInvoicePayment(BuildingProfessionApplicationModel buildingApplicationModel, NewInvoicePaymentRequestModel invoicePaymentRequest);
        Task UpdateInvoicePayment(InvoicePaidEventData invoicePaidEventData);
    }
    public class PaymentService : IPaymentService
    {

        private readonly IntegrationsOptions integrationOptions;
        private readonly SwaOptions swaOptions;
        private readonly IDynamicsService dynamicsService;
        private readonly IPaymentMapper paymentMapper;


        public PaymentService(IOptions<IntegrationsOptions> integrationOptions, IOptions<SwaOptions> swaOptions, IDynamicsService dynamicsService, IPaymentMapper paymentMapper)
        {
            this.integrationOptions = integrationOptions.Value;
            this.swaOptions = swaOptions.Value;
            this.dynamicsService = dynamicsService;
            this.paymentMapper = paymentMapper;
        }


        public NewCardPaymentRequest BuildPaymentRequest(BuildingProfessionApplicationModel model)
        {
            var reference = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..22], @"\W", "0");
            return new NewCardPaymentRequest
            {
                Reference = reference,
                Description = $"Payment for application {model.Id}",
                Amount = Convert.ToInt32(integrationOptions.PaymentAmount),
                ReturnUrl = $"{swaOptions.Url}/application/{model.Id}/application-submission/payment/confirm?reference={reference}",

                Environment = integrationOptions.Environment,
                Application = "rbiportal",
                ApplicationId = model.Id,

                CardHolder = new GovukPaymentCardHolderDetails
                {
                    Name = model.PersonalDetails.ApplicantName.FirstName + " " + model.PersonalDetails.ApplicantName.LastName,
                    Email = model.PersonalDetails.ApplicantEmail.Email,
                    Address = new GovukPaymentCardHolderAddress
                    {
                        Line1 = model.PersonalDetails.ApplicantAddress.Address,
                        Line2 = model.PersonalDetails.ApplicantAddress.AddressLineTwo,
                        Postcode = model.PersonalDetails.ApplicantAddress.Postcode,
                        City = model.PersonalDetails.ApplicantAddress.Town,
                    }
                }
            };
        }

        public async Task<PaymentResponseModel> CreateCardPayment(NewCardPaymentRequest newCardPaymentRequest)
        {
            var response = await integrationOptions.CommonAPIEndpoint
                .AppendPathSegments("api", "CreateCardPayment")
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .PostJsonAsync(newCardPaymentRequest)
                .ReceiveJson<PaymentResponseModel>();

            return response;
        }


        public async Task<PaymentResponseModel> GetPaymentStatus(string referenceNumber)
        {
            var response = await integrationOptions.CommonAPIEndpoint
                .AppendPathSegments("api", "GetPaymentStatus", referenceNumber)
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .GetJsonAsync<PaymentResponseModel>();

            return response;
        }


        public async Task NewInvoicePayment(BuildingProfessionApplicationModel buildingProfessionApplicationModel, NewInvoicePaymentRequestModel invoicePaymentRequest)
        {
            var invoiceContact = await dynamicsService.GetOrCreateInvoiceContactAsync(invoicePaymentRequest);
            var dynamicsApplication = await dynamicsService.GetBuildingProfessionApplicationUsingId(buildingProfessionApplicationModel.Id);

            var dynamicsPayment = await dynamicsService.CreatePaymentAsync(paymentMapper.ToDynamics(buildingProfessionApplicationModel.Id, invoiceContact, invoicePaymentRequest), buildingProfessionApplicationModel.Id);


            var invoicePaymentResponse = await SendCreateInvoiceRequest(
                integrationOptions,
                new CreateInvoiceRequest
                {
                    Amount = Math.Round((float)integrationOptions.PaymentAmount / 100, 2),
                    PaymentId = dynamicsPayment.bsr_paymentid,
                    Name = invoicePaymentRequest.Name,
                    Email = invoicePaymentRequest.Email,
                    AddressLine1 = invoicePaymentRequest.AddressLine1,
                    AddressLine2 = invoicePaymentRequest.AddressLine2,
                    Town = invoicePaymentRequest.Town,
                    Postcode = invoicePaymentRequest.Postcode,
                    Application = "rbiportal",
                    Description = $"Building Professional Application: {buildingProfessionApplicationModel.Id}",
                    Title = "RBI",
                    OrderNumber = invoicePaymentRequest.OrderNumber,
                    CustomerId = invoiceContact.contactid.ToUpper(),
                    Environment = integrationOptions.Environment
                }
                );

            await dynamicsService.UpdateInvoicePaymentAsync(paymentMapper.ToDynamics(dynamicsPayment.bsr_paymentid, invoicePaymentResponse));
        }

        public async Task<InvoiceData> SendCreateInvoiceRequest(IntegrationsOptions integrationOptions, CreateInvoiceRequest invoiceRequest)
        {
            return await integrationOptions.CommonAPIEndpoint
                .AppendPathSegments("api", "CreateInvoice")
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .AllowAnyHttpStatus()
                .PostJsonAsync(invoiceRequest).ReceiveJson<InvoiceData>();
        }

        public async Task UpdateInvoicePayment(InvoicePaidEventData invoicePaidEventData)
        {
            var dynamicsPaymentId = invoicePaidEventData.Data.InvoiceData.InvoiceMetadata.PaymentId;
            var invoiceData = invoicePaidEventData.Data.InvoiceData;

            var dynamicsPayment = new DynamicsPayment
            {
                bsr_paymentid = dynamicsPaymentId,
                bsr_govukpaystatus = invoiceData.Status == "paid" ? "success" : invoiceData.Status,
                bsr_timeanddateoftransaction = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            };

            if (invoiceData.Status == "paid")
            {
                dynamicsPayment = dynamicsPayment with { bsr_paymentreconciliationstatus = DynamicsPaymentReconciliationStatus.Successful };
            }

            await dynamicsService.UpdateInvoicePaymentAsync(dynamicsPayment);

        }
    }
}
