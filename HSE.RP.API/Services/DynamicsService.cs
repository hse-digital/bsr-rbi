using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Util;
using HSE.RP.API.Extensions;
using HSE.RP.API.Model;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.Domain.DynamicsDefinitions;
using HSE.RP.Domain.Entities;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Services
{
    public class DynamicsService
    {
        private readonly DynamicsModelDefinitionFactory dynamicsModelDefinitionFactory;
        private readonly SwaOptions swaOptions;
        private readonly DynamicsApi dynamicsApi;
        private readonly DynamicsOptions dynamicsOptions;

        public DynamicsService(DynamicsModelDefinitionFactory dynamicsModelDefinitionFactory, IOptions<DynamicsOptions> dynamicsOptions, IOptions<SwaOptions> swaOptions, DynamicsApi dynamicsApi)
        {
            this.dynamicsModelDefinitionFactory = dynamicsModelDefinitionFactory;
            this.dynamicsApi = dynamicsApi;
            this.swaOptions = swaOptions.Value;
            this.dynamicsOptions = dynamicsOptions.Value;
        }

        public async Task SendVerificationEmail(EmailVerificationModel emailVerificationModel, string otpToken)
        {
            await dynamicsOptions.EmailVerificationFlowUrl.PostJsonAsync(new
            {
                emailAddress = emailVerificationModel.EmailAddress,
                otp = otpToken,
                hrbRegUrl = swaOptions.Url
            });
        }

        public async Task<BuildingProfessionApplicationModel> RegisterNewBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel)
        {
            var contact = await CreateContactAsync(buildingProfessionApplicationModel);
            var buildingProfessionApplication = await CreateBuildingProfessionApplicationAsync(buildingProfessionApplicationModel: buildingProfessionApplicationModel, contact);
            var dynamicsContact = await dynamicsApi.Get<DynamicsContact>($"contacts({contact.Id})");

            await dynamicsApi.Update($"contacts({dynamicsContact.contactid})", dynamicsContact with {
                bsr_buildingprofessionapplicationid = $"/bsr_buildingprofessionapplications({buildingProfessionApplication.Id})" 
            });

            var dynamicsBuildingProfessionApplication = await dynamicsApi.Get<DynamicsBuildingProfessionApplication>($"bsr_buildingprofessionapplications({buildingProfessionApplication.Id})");

            return buildingProfessionApplicationModel with { Id = dynamicsBuildingProfessionApplication.bsr_buildingproappid };
        }

        private async Task<Contact> CreateContactAsync(BuildingProfessionApplicationModel model)
        {
            var modelDefinition = dynamicsModelDefinitionFactory.GetDefinitionFor<Contact, DynamicsContact>();
            var contact = new Contact(FirstName: model.PersonalDetails.ApplicantName.FirstName ?? "",
                                      LastName: model.PersonalDetails.ApplicantName.LastName ?? "",
                                      PhoneNumber: model.PersonalDetails.ApplicantPhone ?? "",
                                      Email: model.PersonalDetails.ApplicantEmail,
                                      jobRoleReferenceId: $"/bsr_jobroles({DynamicsJobRole.Ids["building_inspector"]})" 
                                      ); ;
            var dynamicsContact = modelDefinition.BuildDynamicsEntity(contact);

            var existingContact = await FindExistingContactAsync(contact.FirstName, contact.LastName, contact.Email, contact.PhoneNumber); 
            if (existingContact == null)
            {
                var response = await dynamicsApi.Create(modelDefinition.Endpoint, dynamicsContact);
                var contactId = ExtractEntityIdFromHeader(response.Headers);
                await AssignContactType(contactId, DynamicsContactTypes.BIApplicant);
                return contact with { Id = contactId };
            }

            return contact with { Id = existingContact.contactid };
        }

        private async Task<BuildingProfessionApplicationModel> CreateBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel, Contact contact)
        {
            var modelDefinition = dynamicsModelDefinitionFactory.GetDefinitionFor<BuildingProfessionApplication, DynamicsBuildingProfessionApplication>();
            var buildingProfessionApplication = new BuildingProfessionApplication(contact.Id, BuildingProfessionTypeCode: BuildingProfessionType.BuildingInspector);
            var dynamicsBuildingProfessionApplication = modelDefinition.BuildDynamicsEntity(buildingProfessionApplication);
            var response = await dynamicsApi.Create(modelDefinition.Endpoint, dynamicsBuildingProfessionApplication);
            var buildingProfessionalApplicationId = ExtractEntityIdFromHeader(response.Headers);


            return buildingProfessionApplicationModel with { Id = buildingProfessionalApplicationId };
        }


        private async Task<DynamicsContact> FindExistingContactAsync(string firstName, string lastName, string email, string phoneNumber)
        {
                       var response = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", new[]
                        {
                        ("$filter", $"firstname eq '{firstName.EscapeSingleQuote()}' and lastname eq '{lastName.EscapeSingleQuote()}' and emailaddress1 eq '{email.EscapeSingleQuote()}' and contains(telephone1, '{phoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')"),
                        ("$expand", "bsr_contacttype_contact")

                    });




            return response.value.FirstOrDefault();
        }


        private string ExtractEntityIdFromHeader(IReadOnlyNameValueList<string> headers)
        {
            var header = headers.FirstOrDefault(x => x.Name == "OData-EntityId");
            var id = Regex.Match(header.Value, @"\((.+)\)");

            return id.Groups[1].Value;
        }


        private async Task AssignContactType(string contactId, string contactTypeId)
        {
            await dynamicsApi.Create($"contacts({contactId})/bsr_contacttype_contact/$ref", new DynamicsContactType
            {
                contactTypeReferenceId = $"{dynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_contacttypes({contactTypeId})"
            });
        }

        public async Task<DynamicsPayment> GetPaymentByReference(string reference)
        {
            var payments = await dynamicsApi.Get<DynamicsResponse<DynamicsPayment>>("bsr_payments", ("$filter", $"bsr_transactionid eq '{reference}'"));
            return payments.value.FirstOrDefault();
        }

        public async Task NewPayment(string applicationId, PaymentResponseModel payment)
        {
            var application = await GetBuildingProfessionApplicationUsingId(applicationId);
            await CreatePayment(new BuildingProfessionApplicationPayment(application.bsr_buildingprofessionapplicationid, payment));
        }

        public async Task<DynamicsBuildingProfessionApplication> GetBuildingProfessionApplicationUsingId(string applicationId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingProfessionApplication>>("bsr_buildingprofessionapplications", new[]
            {
            ("$filter", $"bsr_buildingproappid eq '{applicationId}'"),
            ("$expand", "bsr_applicantid_contact")
            });

            return response.value.FirstOrDefault();
        }

        public async Task<DynamicsContact> GetContactUsingId(string contactId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>($"bsr_buildingprofessionapplications({contactId})");

            return response.value.FirstOrDefault();
        }


        public async Task CreatePayment(BuildingProfessionApplicationPayment buildingProfessionApplicationPayment)
        {
            var payment = buildingProfessionApplicationPayment.Payment;
            var existingPayment = await dynamicsApi.Get<DynamicsResponse<DynamicsPayment>>("bsr_payments", ("$filter", $"bsr_service eq 'Regulating Profession Application' and bsr_transactionid eq '{payment.Reference}'"));
            if (!existingPayment.value.Any())
            {
                await dynamicsApi.Create("bsr_payments", new DynamicsPayment
                {
                    buildingProfessionApplicationReferenceId = $"/bsr_buildingprofessionapplications({buildingProfessionApplicationPayment.BuildingProfessionApplicationId})",
                    bsr_lastfourdigitsofcardnumber = payment.LastFourDigitsCardNumber,
                    bsr_timeanddateoftransaction = payment.CreatedDate,
                    bsr_transactionid = payment.Reference,
                    bsr_service = "RBI",
                    bsr_cardexpirydate = payment.CardExpiryDate,
                    bsr_billingaddress = string.Join(", ", new[] { payment.AddressLineOne, payment.AddressLineTwo, payment.Postcode, payment.City, payment.Country }.Where(x => !string.IsNullOrWhiteSpace(x))),
                    bsr_cardbrandegvisa = payment.CardBrand,
                    bsr_cardtypecreditdebit = payment.CardType == "debit" ? DynamicsPaymentCardType.Debit : DynamicsPaymentCardType.Credit,
                    bsr_amountpaid = Math.Round((float)payment.Amount / 100, 2),
                    bsr_govukpaystatus = payment.Status,
                    bsr_govukpaymentid = payment.PaymentId
                });
            }
            else
            {
                var dynamicsPayment = existingPayment.value[0];
                await dynamicsApi.Update($"bsr_payments({dynamicsPayment.bsr_paymentid})", new DynamicsPayment
                {
                    bsr_timeanddateoftransaction = payment.CreatedDate,
                    bsr_govukpaystatus = payment.Status,
                    bsr_cardexpirydate = payment.CardExpiryDate,
                    bsr_billingaddress = string.Join(", ", new[] { payment.AddressLineOne, payment.AddressLineTwo, payment.Postcode, payment.City, payment.Country }.Where(x => !string.IsNullOrWhiteSpace(x))),
                    bsr_cardbrandegvisa = payment.CardBrand,
                    bsr_cardtypecreditdebit = payment.CardType == "debit" ? DynamicsPaymentCardType.Debit : DynamicsPaymentCardType.Credit,
                    bsr_lastfourdigitsofcardnumber = payment.LastFourDigitsCardNumber,
                    bsr_amountpaid = Math.Round((float)payment.Amount / 100, 2)
                });
            }
        }

        public async Task UpdateBuildingProfessionApplication(DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplication, DynamicsBuildingProfessionApplication buildingProfessionApplication)
        {
            try
            {
                var result = await dynamicsApi.Update($"bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid})", buildingProfessionApplication);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }   

        }

        public async Task<List<DynamicsPayment>> GetPayments(string applicationNumber)
        {
            var buildingProfessionApplication = await GetBuildingProfessionApplicationUsingId(applicationNumber);
            if (buildingProfessionApplication == null)
                return new List<DynamicsPayment>();

            var payments = await dynamicsApi.Get<DynamicsResponse<DynamicsPayment>>("bsr_payments", ("$filter", $"_bsr_buildingprofessionapplicationid_value eq '{buildingProfessionApplication.bsr_buildingproappid}'"));

            return payments.value;
        }


    }


}

