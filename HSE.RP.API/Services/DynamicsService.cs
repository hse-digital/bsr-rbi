using Flurl;
using Flurl.Http;
using Flurl.Util;
using HSE.RP.API.Extensions;
using HSE.RP.API.Mappers;
using HSE.RP.API.Model;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsDataExport;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Models.LocalAuthority;
using HSE.RP.API.Models.Payment;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.Domain.DynamicsDefinitions;
using HSE.RP.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Web;

namespace HSE.RP.API.Services
{
    public interface IDynamicsService
    {

        Task<DynamicsPayment> CreatePaymentAsync(DynamicsPayment dynamicsPayment, string ApplicationId);

        Task<bool> CheckDupelicateBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel);
        Task<DynamicsBuildingProfessionApplication> CheckDupelicateBuildingProfessionApplicationUsingCosmosIdAsync(string cosmosId);

        Task<BuildingInspectorEmploymentDetail> CreateOrUpdateBuildingInspectorEmploymentDetails(BuildingInspectorEmploymentDetail buildingInspectorEmploymentDetail);
        Task<BuildingInspectorProfessionalBodyMembership> CreateOrUpdateBuildingInspectorProfessionalBodyMembership(BuildingInspectorProfessionalBodyMembership buildingInspectorProfessionalBodyMembership);
        Task<DynamicsAccount> CreateOrUpdateEmployer(BuildingProfessionApplicationModel buildingProfessionApplicationModel);
        Task<BuildingInspectorRegistrationActivity> CreateOrUpdateRegistrationActivity(BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity);
        Task<BuildingInspectorRegistrationClass> CreateOrUpdateRegistrationClass(BuildingInspectorRegistrationClass buildingInspectorRegistrationClass);
        Task<BuildingInspectorRegistrationCountry> CreateOrUpdateRegistrationCountry(BuildingInspectorRegistrationCountry buildingInspectorRegistrationCountry);
        Task CreatePayment(BuildingProfessionApplicationPayment buildingProfessionApplicationPayment);
        Task<List<DynamicsBuildingInspectorProfessionalBodyMembership>> GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId(string applicationId);
        Task<DynamicsBuildingProfessionApplication> GetBuildingProfessionApplicationUsingId(string applicationId);
        Task<DynamicsContact> GetContactUsingId(string contactId);
        Task<DynamicsYear> GetDynamicsYear(string year);
        Task<DynamicsBuildingInspectorEmploymentDetail> GetEmploymentDetailsUsingId(string applicationId);
        Task<DynamicsPayment> GetPaymentByReference(string reference);
        Task<List<DynamicsPayment>> GetPayments(string applicationNumber);
        Task<string[]> GetPublicSectorBodies();
        Task<List<DynamicsBuildingInspectorRegistrationActivity>> GetRegistrationActivitiesUsingApplicationId(string applicationId);
        Task<List<DynamicsBuildingInspectorRegistrationClass>> GetRegistrationClassesUsingApplicationId(string applicationId);
        Task<List<DynamicsBuildingInspectorRegistrationCountry>> GetRegistrationCountriesUsingApplicationId(string applicationId);
        Task NewPayment(string applicationId, PaymentResponseModel payment);
        Task<BuildingProfessionApplicationModel> RegisterNewBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel);
        Task<DynamicsOrganisationsSearchResponse> SearchLocalAuthorities(string authorityName);
        Task<DynamicsOrganisationsSearchResponse> SearchSocialHousingOrganisations(string authorityName);
        Task SendVerificationEmail(EmailVerificationModel emailVerificationModel, string otpToken);
        Task UpdateBuildingProfessionApplication(DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplication, DynamicsBuildingProfessionApplication buildingProfessionApplication);
        Task UpdateBuildingProfessionApplicationCompetency(DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplication, DynamicsBuildingProfessionApplication buildingProfessionApplication);
        Task UpdateContact(DynamicsContact dynamicsContact, DynamicsContact contact);
        Task<DynamicsPayment> CreateDynamicsPaymentAsync(DynamicsPayment dynamicsPayment);
        Task UpdateDynamicsPaymentAsync(DynamicsPayment dynamicsPayment);
        Task<DynamicsContact> GetOrCreateInvoiceContactAsync(NewInvoicePaymentRequestModel invoicePaymentRequest);
        Task<DynamicsResponse<DynamicsContact>> GetContactsAsync(string firstName, string lastName, string emailAddress);
        Task UpdateInvoicePaymentAsync(DynamicsPayment dynamicsPayment);

        Task<InvoiceData> SendCreateInvoiceRequest(IntegrationsOptions integrationOptions, CreateInvoiceRequest invoiceRequest);
        Task<DynamicsContact> CreateNewContactAsync(DynamicsContact contact, bool returnObjectResponse = false);
        Task<Contact> CreateContactAsync(BuildingProfessionApplicationModel model);
        Task<BuildingProfessionApplicationModel> CreateBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel, Contact contact);
        Task<DynamicsContact> FindExistingContactAsync(string firstName, string lastName, string email, string phoneNumber);
        Task<DynamicsBuildingInspectorRegistrationClass> FindExistingBuildingInspectorRegistrationClass(string classId, string buidingProfessionApplicationId);
        Task<DynamicsBuildingInspectorProfessionalBodyMembership> FindExistingBuildingProfessionalBodyMembership(string membershipBodyId, string buidingProfessionApplicationId);
        Task<DynamicsBuildingInspectorRegistrationCountry> FindExistingBuildingInspectorRegistrationCountry(string countryId, string buidingProfessionApplicationId);
        Task<DynamicsBuildingInspectorRegistrationActivity> FindExistingBuildingInspectorRegistrationActivity(string activityId, string categoryId, string buidingProfessionApplicationId);
        string ExtractEntityIdFromHeader(IReadOnlyNameValueList<string> headers);
        Task AssignContactType(string contactId, string contactTypeId);
        Task<DynamicsOrganisationsSearchResponse> SearchOrganisations(string authorityName, string accountTypeId);
        Task<List<ApplicantEmploymentDetail>> GetDynamicsModifiedRBIApplicationEmploymentDetails();
        Task<List<ApplicantClassDetails>> GetDynamicsModifiedRBIApplicationClassDetails();
        Task<List<ApplicantActivityDetails>> GetDynamicsModifiedRBIApplicationActivityDetails();
        Task<List<ApplicantCountryDetails>> GetDynamicsModifiedRBIApplicationCountryDetails();
        Task<List<DynamicsBuildingProfessionRegisterApplication>> GetDynamicsRBIApplicationsToProcess();
        Task<DynamicsBuildingProfessionRegisterApplication> GetDynamicsRBIApplicationData(string applicationId);
        Task<List<DynamicsBuildingProfessionRegisterApplication>> GetDynamicsModifiedRBIApplicationData();
    }
    public class DynamicsService : IDynamicsService
    {
        private readonly DynamicsModelDefinitionFactory dynamicsModelDefinitionFactory;
        private readonly SwaOptions swaOptions;
        private readonly DynamicsApi dynamicsApi;
        private readonly DynamicsOptions dynamicsOptions;
        private readonly FeatureOptions featureOptions;

        public DynamicsService(DynamicsModelDefinitionFactory dynamicsModelDefinitionFactory, IOptions<FeatureOptions> featureOptions, IOptions<DynamicsOptions> dynamicsOptions, IOptions<SwaOptions> swaOptions, DynamicsApi dynamicsApi)
        {
            this.dynamicsModelDefinitionFactory = dynamicsModelDefinitionFactory;
            this.dynamicsApi = dynamicsApi;
            this.swaOptions = swaOptions.Value;
            this.dynamicsOptions = dynamicsOptions.Value;
            this.featureOptions = featureOptions.Value;
        }


        public async Task<List<DynamicsBuildingProfessionRegisterApplication>> GetDynamicsRBIApplicationsToProcess()
        {

            var result = new List<DynamicsBuildingProfessionRegisterApplication>();

            var DynamicsRBIApplications = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingProfessionRegisterApplication>>("bsr_buildingprofessionapplications", new[]
            {
                    ("$select", $"bsr_buildingprofessionapplicationid,bsr_buildingproappid"),
                    ("$filter", $"(statuscode eq 760810005) and ((Microsoft.Dynamics.CRM.In(PropertyName='bsr_regulatorydecisionstatus',PropertyValues=['760810000','760810002']))) and (bsr_buildingprofessiontypecode eq 760810000) and (bsr_applicantid_contact/contactid ne null)")
                });

            result.AddRange(DynamicsRBIApplications.value);

            while (DynamicsRBIApplications.nextLink != null)
            {
                DynamicsRBIApplications = await dynamicsApi.GetNextPage<DynamicsResponse<DynamicsBuildingProfessionRegisterApplication>>(DynamicsRBIApplications.nextLink);
                result.AddRange(DynamicsRBIApplications.value);
            }

            return result;

        }

        public async Task<DynamicsBuildingProfessionRegisterApplication> GetDynamicsRBIApplicationData(string applicationId)
        {


            var DynamicsRBIApplications = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingProfessionRegisterApplication>>("bsr_buildingprofessionapplications", new[]
            {
                    ("$select", $"bsr_buildingproappid,bsr_buildingprofessiontypecode,bsr_registrationcommencementdate,bsr_decisioncondition,bsr_decisionreason,bsr_regulatorydecisionstatus,bsr_reviewdecision,bsr_buildingprofessionapplicationid"),
                    ("$expand", $"bsr_applicantid_contact($select=firstname,lastname),bsr_biemploymentdetail_buildingprofessionappl($select=bsr_biemploymentdetailid, _bsr_employmenttypeid_value;$expand=bsr_biemployerid_account($select=name,address1_composite);$filter=(statuscode eq 1)),bsr_bsr_biregclass_buildingprofessionapplicat($select=bsr_biregclassid,statuscode;$expand=bsr_biclassid($select=bsr_name);$filter=(statuscode eq 760810002)),bsr_biregactivity_buildingprofessionapplicati($select=bsr_biregactivityid,statuscode;$expand=bsr_biactivityid($select=bsr_name),bsr_bibuildingcategoryid($select=bsr_name);$filter=(statuscode eq 760810002)),bsr_bsr_biregcountry_buildingprofessionapplic($select=bsr_biregcountryid;$expand=bsr_countryid($select=bsr_name);$filter=(statecode eq 0))"),
                    ("$filter", $"(statuscode eq 760810005) and ((Microsoft.Dynamics.CRM.In(PropertyName='bsr_regulatorydecisionstatus',PropertyValues=['760810000','760810002']))) and (bsr_buildingprofessiontypecode eq 760810000) and (bsr_buildingprofessionapplicationid eq '{applicationId}') and (bsr_applicantid_contact/contactid ne null)")
                });
            return DynamicsRBIApplications.value.FirstOrDefault();

        }


        public async Task<List<DynamicsBuildingProfessionRegisterApplication>> GetDynamicsModifiedRBIApplicationData()
        {
            var result = new List<DynamicsBuildingProfessionRegisterApplication>();


            var DynamicsRBIApplications = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingProfessionRegisterApplication>>("bsr_buildingprofessionapplications", new[]
            {
                    ("$select", $"bsr_buildingproappid,bsr_buildingprofessiontypecode,bsr_registrationcommencementdate,bsr_decisioncondition,bsr_decisionreason,bsr_regulatorydecisionstatus,bsr_reviewdecision,bsr_buildingprofessionapplicationid"),
                    ("$expand", $"bsr_applicantid_contact($select=firstname,lastname),bsr_biemploymentdetail_buildingprofessionappl($select=bsr_biemploymentdetailid, _bsr_employmenttypeid_value;$expand=bsr_biemployerid_account($select=name,address1_composite);$filter=(statuscode eq 1)),bsr_bsr_biregclass_buildingprofessionapplicat($select=bsr_biregclassid,statuscode;$expand=bsr_biclassid($select=bsr_name);$filter=(statuscode eq 760810002)),bsr_biregactivity_buildingprofessionapplicati($select=bsr_biregactivityid,statuscode;$expand=bsr_biactivityid($select=bsr_name),bsr_bibuildingcategoryid($select=bsr_name);$filter=(statuscode eq 760810002)),bsr_bsr_biregcountry_buildingprofessionapplic($select=bsr_biregcountryid;$expand=bsr_countryid($select=bsr_name);$filter=(statecode eq 0))"),
                    ("$filter", $"(statuscode eq 760810005) and ((Microsoft.Dynamics.CRM.In(PropertyName='bsr_regulatorydecisionstatus',PropertyValues=['760810000','760810002']))) and (bsr_buildingprofessiontypecode eq 760810000) and (modifiedon ge {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")}) and (bsr_applicantid_contact/contactid ne null)")
                });

            result.AddRange(DynamicsRBIApplications.value);

            while (DynamicsRBIApplications.nextLink != null)
            {
                DynamicsRBIApplications = await dynamicsApi.GetNextPage<DynamicsResponse<DynamicsBuildingProfessionRegisterApplication>>(DynamicsRBIApplications.nextLink);
                result.AddRange(DynamicsRBIApplications.value);
            }

            return result;

        }

        public async Task<List<ApplicantEmploymentDetail>> GetDynamicsModifiedRBIApplicationEmploymentDetails()
        {

            var result = new List<ApplicantEmploymentDetail>();

            var employmentDetails = await dynamicsApi.Get<DynamicsResponse<ApplicantEmploymentDetail>>("bsr_biemploymentdetails", new[]
            {
                    ("$select", $"bsr_biemploymentdetailid,_bsr_biapplicationid_value,_bsr_employmenttypeid_value"),
                    ("$expand", $"bsr_biemployerid_account($select=address1_composite,name)"),
                    ("$filter", $"(statuscode eq 1) and (bsr_biemployerid_account/modifiedon ge {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")} or modifiedon ge {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")})")
                });

            result.AddRange(employmentDetails.value);

            while (employmentDetails.nextLink != null)
            {
                employmentDetails = await dynamicsApi.GetNextPage<DynamicsResponse<ApplicantEmploymentDetail>>(employmentDetails.nextLink);
                result.AddRange(employmentDetails.value);
            }

            return result;

        }

        public async Task<List<ApplicantClassDetails>> GetDynamicsModifiedRBIApplicationClassDetails()
        {

            var result = new List<ApplicantClassDetails>();

            var classDetails = await dynamicsApi.Get<DynamicsResponse<ApplicantClassDetails>>("bsr_biregclasses", new[]
            {
                    ("$select", $"bsr_biregclassid,statuscode,_bsr_biapplicationid_value"),
                    ("$expand", $"bsr_biclassid($select=bsr_name)"),
                    ("$filter", $"(statuscode eq 760810002) and (modifiedon ge {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")})")
                });

            result.AddRange(classDetails.value);

            while (classDetails.nextLink != null)
            {
                classDetails = await dynamicsApi.GetNextPage<DynamicsResponse<ApplicantClassDetails>>(classDetails.nextLink);
                result.AddRange(classDetails.value);
            }

            return result;

        }


        public async Task<List<ApplicantActivityDetails>> GetDynamicsModifiedRBIApplicationActivityDetails()
        {

            var result = new List<ApplicantActivityDetails>();

            var activityDetails = await dynamicsApi.Get<DynamicsResponse<ApplicantActivityDetails>>("bsr_biregactivities", new[]
            {
                    ("$select", $"bsr_biregactivityid,statuscode,_bsr_biapplicationid_value"),
                    ("$expand", $"bsr_biactivityid($select=bsr_name),bsr_bibuildingcategoryid($select=bsr_name)"),
                    ("$filter", $"(statuscode eq 760810002) and (modifiedon ge {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")})")
                });

            result.AddRange(activityDetails.value);

            while (activityDetails.nextLink != null)
            {
                activityDetails = await dynamicsApi.GetNextPage<DynamicsResponse<ApplicantActivityDetails>>(activityDetails.nextLink);
                result.AddRange(activityDetails.value);
            }

            return result;

        }

        public async Task<List<ApplicantCountryDetails>> GetDynamicsModifiedRBIApplicationCountryDetails()
        {

            var result = new List<ApplicantCountryDetails>();

            var countryDetails = await dynamicsApi.Get<DynamicsResponse<ApplicantCountryDetails>>("bsr_biregcountries", new[]
            {
                    ("$select", $"bsr_biregcountryid,_bsr_biapplicationid_value"),
                    ("$expand", $"bsr_countryid($select=bsr_name)"),
                    ("$filter", $"(statecode eq 0) and (modifiedon ge {DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")})")
                });

            result.AddRange(countryDetails.value);

            while (countryDetails.nextLink != null)
            {
                countryDetails = await dynamicsApi.GetNextPage<DynamicsResponse<ApplicantCountryDetails>>(countryDetails.nextLink);
                result.AddRange(countryDetails.value);
            }

            return result;

        }



        public async Task<DynamicsPayment> CreatePaymentAsync(DynamicsPayment dynamicsPayment, string ApplicationId)
        {
            var existingPayment = await dynamicsApi.Get<DynamicsResponse<DynamicsPayment>>("bsr_payments", ("$filter", $"bsr_service eq 'RBI' and bsr_transactionid eq '{dynamicsPayment.bsr_transactionid}'"));
            if (existingPayment.value.Count == 0)
            {

                var pending = await dynamicsApi.Get<DynamicsResponse<DynamicsPayment>>("bsr_payments", ("$filter", $"bsr_service eq 'RBI' and bsr_govukpaystatus eq 'created' and _bsr_buildingprofessionapplicationid_value eq '{ApplicationId}'"));

                foreach (var record in pending.value)
                {
                    await dynamicsApi.Update($"bsr_payments({record.bsr_paymentid})", record with
                    {
                        bsr_govukpaystatus = "failed",
                        bsr_paymentreconciliationstatus = DynamicsPaymentReconciliationStatus.FailedPayment,
                    });
                }

                dynamicsPayment = await CreateDynamicsPaymentAsync(dynamicsPayment);
            }
            else
            {
                await UpdateDynamicsPaymentAsync(dynamicsPayment);
            }
            return dynamicsPayment;
        }

        public async Task<DynamicsPayment> CreateDynamicsPaymentAsync(DynamicsPayment dynamicsPayment)
        {
            var response = await dynamicsApi.Create("bsr_payments", dynamicsPayment);
            var paymentId = ExtractEntityIdFromHeader(response.Headers);
            return dynamicsPayment with { bsr_paymentid = paymentId };
        }

        public async Task UpdateDynamicsPaymentAsync(DynamicsPayment dynamicsPayment)
        {
            await dynamicsApi.Update($"bsr_payments({dynamicsPayment.bsr_paymentid})", dynamicsPayment);

        }

        public async Task UpdateInvoicePaymentAsync(DynamicsPayment dynamicsPayment)
        {
            await dynamicsApi.Update($"bsr_payments({dynamicsPayment.bsr_paymentid})", dynamicsPayment);
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

        public async Task<InvoiceData> SendCreateInvoiceRequest(IntegrationsOptions integrationOptions, CreateInvoiceRequest invoiceRequest)
        {
            return await integrationOptions.CommonAPIEndpoint
                .AppendPathSegments("api", "CreateInvoice")
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .AllowAnyHttpStatus()
                .PostJsonAsync(invoiceRequest).ReceiveJson<InvoiceData>();

        }

        public async Task<DynamicsContact> GetOrCreateInvoiceContactAsync(NewInvoicePaymentRequestModel invoicePaymentRequest)
        {
            var splitName = invoicePaymentRequest.Name.Split(' ');
            var lastName = string.Join(' ', splitName.Skip(1));
            var responseContact = await GetContactsAsync(splitName[0], lastName, invoicePaymentRequest.Email);
            var records = responseContact.value;
            if (!records.Any())
            {
                var contact = await CreateNewContactAsync(new DynamicsContact
                {
                    firstname = splitName[0],
                    lastname = lastName,
                    emailaddress1 = invoicePaymentRequest.Email,
                    address1_line1 = invoicePaymentRequest.AddressLine1,
                    address1_line2 = invoicePaymentRequest.AddressLine2,
                    address1_postalcode = invoicePaymentRequest.Postcode,
                    address1_city = invoicePaymentRequest.Town
                });
                return contact;
            }

            return records.FirstOrDefault();
        }

        public async Task<DynamicsContact> CreateNewContactAsync(DynamicsContact contact, bool returnObjectResponse = false)
        {
            var response = await dynamicsApi.Create("contacts", contact, returnObjectResponse);
            var contactId = ExtractEntityIdFromHeader(response.Headers);

            return contact with { contactid = contactId };
        }


        public async Task<DynamicsResponse<DynamicsContact>> GetContactsAsync(string firstName, string lastName, string emailAddress)
        {
            return await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", ("$filter", $"firstname eq '{firstName}' and lastname eq '{lastName}' and emailaddress1 eq '{emailAddress}' "));
        }

        public async Task<BuildingProfessionApplicationModel> RegisterNewBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel)
        {
            var contact = await CreateContactAsync(buildingProfessionApplicationModel);

            var buildingProfessionApplication = await CreateBuildingProfessionApplicationAsync(buildingProfessionApplicationModel: buildingProfessionApplicationModel, contact);

            var dynamicsContact = await dynamicsApi.Get<DynamicsContact>($"contacts({contact.Id})");

            await dynamicsApi.Update($"contacts({dynamicsContact.contactid})", dynamicsContact with
            {
                bsr_buildingprofessionapplicationid = $"/bsr_buildingprofessionapplications({buildingProfessionApplication.Id})"
            });

            var dynamicsBuildingProfessionApplication = await dynamicsApi.Get<DynamicsBuildingProfessionApplication>($"bsr_buildingprofessionapplications({buildingProfessionApplication.Id})");
            return buildingProfessionApplicationModel with
            {
                Id = dynamicsBuildingProfessionApplication.bsr_buildingproappid,
            };
        }



        public async Task<Contact> CreateContactAsync(BuildingProfessionApplicationModel model)
        {
            var modelDefinition = dynamicsModelDefinitionFactory.GetDefinitionFor<Contact, DynamicsContact>();
            var contact = new Contact(FirstName: model.PersonalDetails.ApplicantName.FirstName ?? "",
                                      LastName: model.PersonalDetails.ApplicantName.LastName ?? "",
                                      PhoneNumber: model.PersonalDetails.ApplicantPhone.PhoneNumber ?? "",
                                      Email: model.PersonalDetails.ApplicantEmail.Email,
                                      jobRoleReferenceId: $"/bsr_jobroles({DynamicsJobRole.Ids["building_inspector"]})"
                                      );
            var dynamicsContact = modelDefinition.BuildDynamicsEntity(contact);


            var existingContact = await FindExistingContactAsync(contact.FirstName, contact.LastName, contact.Email, contact.PhoneNumber);


            if (existingContact == null)
            {
                var response = await dynamicsApi.Create(modelDefinition.Endpoint, dynamicsContact);
                var contactId = ExtractEntityIdFromHeader(response.Headers);
                await AssignContactType(contactId, DynamicsContactTypes.BIApplicant);
                return contact with { Id = contactId };
            }

            await AssignContactType(existingContact.contactid, DynamicsContactTypes.BIApplicant);

            return contact with { Id = existingContact.contactid };
        }

        public async Task<BuildingProfessionApplicationModel> CreateBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel, Contact contact)
        {

            try
            {
                var modelDefinition = dynamicsModelDefinitionFactory.GetDefinitionFor<BuildingProfessionApplication, DynamicsBuildingProfessionApplication>();
                var buildingProfessionApplication = new BuildingProfessionApplication(ContactId: contact.Id, BuildingProfessionTypeCode: BuildingProfessionType.BuildingInspector, StatusCode: BuildingProfessionApplicationStatus.New, CosmosId: buildingProfessionApplicationModel.CosmosId);
                var dynamicsBuildingProfessionApplication = modelDefinition.BuildDynamicsEntity(buildingProfessionApplication);
                var response = await dynamicsApi.Create(modelDefinition.Endpoint, dynamicsBuildingProfessionApplication);
                var buildingProfessionalApplicationId = ExtractEntityIdFromHeader(response.Headers);


                return buildingProfessionApplicationModel with { Id = buildingProfessionalApplicationId };
            }
            catch (FlurlHttpException ex) when (ex.StatusCode == 412)
            {
                //Check for existing application and return the id
                var existingBuildingProfessionApplication = await CheckDupelicateBuildingProfessionApplicationUsingCosmosIdAsync(buildingProfessionApplicationModel.CosmosId);

                if (existingBuildingProfessionApplication != null)
                {
                    return buildingProfessionApplicationModel with { Id = existingBuildingProfessionApplication.bsr_buildingprofessionapplicationid };
                }

                throw;
            }
        }




        public async Task<DynamicsContact> FindExistingContactAsync(string firstName, string lastName, string email, string phoneNumber)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", new[]
             {
                        ("$filter", $"firstname eq '{firstName.EscapeSingleQuote()}' and lastname eq '{lastName.EscapeSingleQuote()}' and statuscode eq 1 and emailaddress1 eq '{email.EscapeSingleQuote()}' and contains(telephone1, '{phoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')"),
                        ("$expand", "bsr_contacttype_contact")

                    });

            return response.value.FirstOrDefault();
        }

        public async Task<DynamicsBuildingInspectorRegistrationClass> FindExistingBuildingInspectorRegistrationClass(string classId, string buidingProfessionApplicationId)
        {
            try
            {
                var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorRegistrationClass>>("bsr_biregclasses", new[]
                 {
                        ("$filter", $"_bsr_biapplicationid_value eq '{buidingProfessionApplicationId}' and _bsr_biclassid_value eq '{classId}'")

                    });

                return response.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<DynamicsBuildingInspectorProfessionalBodyMembership> FindExistingBuildingProfessionalBodyMembership(string membershipBodyId, string buidingProfessionApplicationId)
        {

            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership>>("bsr_biprofessionalmemberships", new[]
             {
                        ("$filter", $"_bsr_biapplicationid_value eq '{buidingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{membershipBodyId}'")

                    });

            return response.value.FirstOrDefault();

        }

        public async Task<DynamicsBuildingInspectorRegistrationCountry> FindExistingBuildingInspectorRegistrationCountry(string countryId, string buidingProfessionApplicationId)
        {


            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorRegistrationCountry>>("bsr_biregcountries", new[]
             {
                        ("$filter", $"_bsr_biapplicationid_value eq '{buidingProfessionApplicationId}' and _bsr_countryid_value eq '{countryId}'")

                    });

            return response.value.FirstOrDefault();


        }

        public async Task<DynamicsBuildingInspectorRegistrationActivity> FindExistingBuildingInspectorRegistrationActivity(string activityId, string categoryId, string buidingProfessionApplicationId)
        {

            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorRegistrationActivity>>("bsr_biregactivities", new[]
             {
                        ("$filter", $"_bsr_biapplicationid_value eq '{buidingProfessionApplicationId}' and _bsr_biactivityid_value eq '{activityId}' and _bsr_bibuildingcategoryid_value eq '{categoryId}'")

                    });

            return response.value.FirstOrDefault();

        }


        public string ExtractEntityIdFromHeader(IReadOnlyNameValueList<string> headers)
        {
            var header = headers.FirstOrDefault(x => x.Name == "OData-EntityId");
            var id = Regex.Match(header.Value, @"\((.+)\)");

            return id.Groups[1].Value;
        }


        public async Task AssignContactType(string contactId, string contactTypeId)
        {
            var contactTypeExists = await dynamicsApi.Get<DynamicsResponse<DynamicsContactType>>($"contacts({contactId})/bsr_contacttype_contact", new[]
            {
                ("$filter", $"bsr_contacttypeid eq '{contactTypeId}'")
            });

            if (contactTypeExists.value.Count == 0)
            {
                await dynamicsApi.Create($"contacts({contactId})/bsr_contacttype_contact/$ref",
                new DynamicsContactType { contactTypeReferenceId = $"{dynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_contacttypes({contactTypeId})" });
            }

        }

        public async Task AssignAccountType(string accountId, string accountTypeId)
        {
            var accountTypeExists = await dynamicsApi.Get<DynamicsResponse<DynamicsAccountTypeRelationship>>($"accounts({accountId})/bsr_Account_bsr_accounttype_bsr_accounttype", new[]
            {
                            ("$filter", $"bsr_accounttypeid eq '{accountTypeId}'")
                        });

            if (accountTypeExists.value.Count == 0)
            {
                await dynamicsApi.Create($"accounts({accountId})/bsr_Account_bsr_accounttype_bsr_accounttype/$ref",
            new DynamicsAccountTypeRelationship { accountTypeReferenceId = $"{dynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_accounttypes({accountTypeId})" });
            }
            

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
            try
            {
                var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingProfessionApplication>>("bsr_buildingprofessionapplications", new[]
                {
                ("$filter", $"bsr_buildingproappid eq '{applicationId}'"),
                ("$expand", "bsr_applicantid_contact,")
                });
                return response.value.FirstOrDefault();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public async Task<DynamicsContact> GetContactUsingId(string contactId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", new[]
            {
            ("$filter", $"contactid eq '{contactId}'")
            });

            return response.value.FirstOrDefault();
        }

        public async Task<List<DynamicsBuildingInspectorRegistrationClass>> GetRegistrationClassesUsingApplicationId(string applicationId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorRegistrationClass>>("bsr_biregclasses", new[]
            {
            ("$filter", $"_bsr_biapplicationid_value eq '{applicationId}'")
            });

            return response.value;
        }
        public async Task<List<DynamicsBuildingInspectorProfessionalBodyMembership>> GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId(string applicationId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership>>("bsr_biprofessionalmemberships", new[]
            {
            ("$filter", $"_bsr_biapplicationid_value eq '{applicationId}'")
            });

            return response.value;
        }

        public async Task<List<DynamicsBuildingInspectorRegistrationActivity>> GetRegistrationActivitiesUsingApplicationId(string applicationId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorRegistrationActivity>>("bsr_biregactivities", new[]
            {
            ("$filter", $"_bsr_biapplicationid_value eq '{applicationId}'")
            });

            return response.value;
        }



        public async Task<List<DynamicsBuildingInspectorRegistrationCountry>> GetRegistrationCountriesUsingApplicationId(string applicationId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorRegistrationCountry>>("bsr_biregcountries", new[]
            {
            ("$filter", $"_bsr_biapplicationid_value eq '{applicationId}'")
            });

            return response.value;
        }

        public async Task CreatePayment(BuildingProfessionApplicationPayment buildingProfessionApplicationPayment)
        {
            var payment = buildingProfessionApplicationPayment.Payment;
            var existingPayment = await dynamicsApi.Get<DynamicsResponse<DynamicsPayment>>("bsr_payments", ("$filter", $"bsr_service eq 'RBI' and bsr_transactionid eq '{payment.Reference}'"));
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

                if (payment.Status == "failed")
                {
                    await dynamicsApi.Update($"bsr_payments({dynamicsPayment.bsr_paymentid})", new DynamicsPayment
                    {
                        bsr_timeanddateoftransaction = payment.CreatedDate,
                        bsr_govukpaystatus = payment.Status,
                        bsr_cardexpirydate = payment.CardExpiryDate,
                        bsr_billingaddress = string.Join(", ", new[] { payment.AddressLineOne, payment.AddressLineTwo, payment.Postcode, payment.City, payment.Country }.Where(x => !string.IsNullOrWhiteSpace(x))),
                        bsr_cardbrandegvisa = payment.CardBrand,
                        bsr_cardtypecreditdebit = payment.CardType == "debit" ? DynamicsPaymentCardType.Debit : DynamicsPaymentCardType.Credit,
                        bsr_lastfourdigitsofcardnumber = payment.LastFourDigitsCardNumber,
                        bsr_amountpaid = Math.Round((float)payment.Amount / 100, 2),
                        bsr_paymentreconciliationstatus = DynamicsPaymentReconciliationStatus.FailedPayment
                    });
                }
                else
                {
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
        }

        public async Task UpdateBuildingProfessionApplication(DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplication, DynamicsBuildingProfessionApplication buildingProfessionApplication)
        {
            try
            {
                var result = await dynamicsApi.Update($"bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid})", buildingProfessionApplication);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task UpdateBuildingProfessionApplicationCompetency(DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplication, DynamicsBuildingProfessionApplication buildingProfessionApplication)
        {
            try
            {
                var result = await dynamicsApi.Update($"bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid})", buildingProfessionApplication);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task UpdateContact(DynamicsContact dynamicsContact, DynamicsContact contact)
        {
            try
            {
                var result = await dynamicsApi.Update($"contacts({dynamicsContact.contactid})", contact);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;

            }

        }

        public async Task<List<DynamicsPayment>> GetPayments(string applicationNumber)
        {
            var buildingProfessionApplication = await GetBuildingProfessionApplicationUsingId(applicationNumber);
            if (buildingProfessionApplication == null)
                return new List<DynamicsPayment>();

            var payments = await dynamicsApi.Get<DynamicsResponse<DynamicsPayment>>("bsr_payments", ("$filter", $"_bsr_buildingprofessionapplicationid_value eq '{buildingProfessionApplication.bsr_buildingprofessionapplicationid}'"));

            return payments.value;
        }

        public async Task<BuildingInspectorRegistrationClass> CreateOrUpdateRegistrationClass(BuildingInspectorRegistrationClass buildingInspectorRegistrationClass)
        {

            var dynamicsBuildingInspectorRegistraionClass = new DynamicsBuildingInspectorRegistrationClass(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationClass.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationClass.ApplicantId})",
                classRef: $"/bsr_biclasses({buildingInspectorRegistrationClass.ClassId})",
                statuscode: buildingInspectorRegistrationClass.StatusCode,
                statecode: buildingInspectorRegistrationClass.StateCode
                );

            //If the registration class has an id then we need to update it
            if (buildingInspectorRegistrationClass.Id is not null)
            {
                var response = await dynamicsApi.Update($"bsr_biregclasses({buildingInspectorRegistrationClass.Id})", dynamicsBuildingInspectorRegistraionClass);
                var buildingInspectorRegistrationClassId = ExtractEntityIdFromHeader(response.Headers);
                return buildingInspectorRegistrationClass with { Id = buildingInspectorRegistrationClassId };
            }
            else
            {
                //Check if an entry for this class already exists
                var existingRegistrationClass = await FindExistingBuildingInspectorRegistrationClass(buildingInspectorRegistrationClass.ClassId, buildingInspectorRegistrationClass.BuildingProfessionApplicationId);

                //If no entry exists then create a new one
                if (existingRegistrationClass == null)
                {
                    var response = await dynamicsApi.Create("bsr_biregclasses", dynamicsBuildingInspectorRegistraionClass);
                    var buildingInspectorRegistrationClassId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorRegistrationClass with { Id = buildingInspectorRegistrationClassId };
                }

                //If an entry exists then update it
                else
                {
                    var response = await dynamicsApi.Update($"bsr_biregclasses({existingRegistrationClass.bsr_biregclassid})", dynamicsBuildingInspectorRegistraionClass);
                    var buildingInspectorRegistrationClassId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorRegistrationClass with { Id = buildingInspectorRegistrationClassId };
                }
            }

        }

        public async Task<BuildingInspectorRegistrationCountry> CreateOrUpdateRegistrationCountry(BuildingInspectorRegistrationCountry buildingInspectorRegistrationCountry)
        {

            var dynamicsBuildingInspectorRegistraionCountry = new DynamicsBuildingInspectorRegistrationCountry(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationCountry.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationCountry.BuildingInspectorId})",
                countryRefId: $"/bsr_countries({buildingInspectorRegistrationCountry.CountryID})",
                statuscode: buildingInspectorRegistrationCountry.StatusCode,
                statecode: buildingInspectorRegistrationCountry.StateCode
                );

            //If the registration class has an id then we need to update it
            if (buildingInspectorRegistrationCountry.Id is not null)
            {
                var response = await dynamicsApi.Update($"bsr_biregcountries({buildingInspectorRegistrationCountry.Id})", dynamicsBuildingInspectorRegistraionCountry);
                var buildingInspectorRegistrationCountryId = ExtractEntityIdFromHeader(response.Headers);
                return buildingInspectorRegistrationCountry with { Id = buildingInspectorRegistrationCountryId };
            }
            else
            {
                //Check if an entry for this class already exists
                var existingRegistrationCountry = await FindExistingBuildingInspectorRegistrationCountry(buildingInspectorRegistrationCountry.CountryID, buildingInspectorRegistrationCountry.BuildingProfessionApplicationId);

                //If no entry exists then create a new one
                if (existingRegistrationCountry == null)
                {
                    var response = await dynamicsApi.Create("bsr_biregcountries", dynamicsBuildingInspectorRegistraionCountry);
                    var buildingInspectorRegistrationCountryId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorRegistrationCountry with { Id = buildingInspectorRegistrationCountryId };
                }
                //If an entry exists then update it
                else
                {
                    var response = await dynamicsApi.Update($"bsr_biregcountries({existingRegistrationCountry.bsr_biregcountryid})", dynamicsBuildingInspectorRegistraionCountry);
                    var buildingInspectorRegistrationCountryId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorRegistrationCountry with { Id = buildingInspectorRegistrationCountryId };
                }
            }

        }

        public async Task<BuildingInspectorRegistrationActivity> CreateOrUpdateRegistrationActivity(BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity)
        {

            var dynamicsBuildingInspectorRegistraionActivity = new DynamicsBuildingInspectorRegistrationActivity(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationActivity.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationActivity.BuildingInspectorId})",
                buildingActivityReferenceId: $"/bsr_biactivities({buildingInspectorRegistrationActivity.ActivityId})",
                buidingCategoryReferenceId: $"/bsr_bibuildingcategories({buildingInspectorRegistrationActivity.BuildingCategoryId})",
                statuscode: buildingInspectorRegistrationActivity.StatusCode,
                statecode: buildingInspectorRegistrationActivity.StateCode
                );

            //If the registration class has an id then we need to update it
            if (buildingInspectorRegistrationActivity.Id is not null)
            {
                var response = await dynamicsApi.Update($"bsr_biregactivities({buildingInspectorRegistrationActivity.Id})", dynamicsBuildingInspectorRegistraionActivity);
                var buildingInspectorRegistrationActivityId = ExtractEntityIdFromHeader(response.Headers);
                return buildingInspectorRegistrationActivity with { Id = buildingInspectorRegistrationActivityId };
            }
            else
            {
                //Check if an entry for this class, category and activity already exists
                var existingRegistrationActivity = await FindExistingBuildingInspectorRegistrationActivity(buildingInspectorRegistrationActivity.ActivityId, buildingInspectorRegistrationActivity.BuildingCategoryId, buildingInspectorRegistrationActivity.BuildingProfessionApplicationId);

                //If no entry exists then create a new one
                if (existingRegistrationActivity == null)
                {
                    var response = await dynamicsApi.Create("bsr_biregactivities", dynamicsBuildingInspectorRegistraionActivity);
                    var buildingInspectorRegistrationActivityId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorRegistrationActivity with { Id = buildingInspectorRegistrationActivityId };
                }
                //If an entry exists then update it
                else
                {
                    var response = await dynamicsApi.Update($"bsr_biregactivities({existingRegistrationActivity.bsr_biregactivityId})", dynamicsBuildingInspectorRegistraionActivity);
                    var buildingInspectorRegistrationActivityId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorRegistrationActivity with { Id = buildingInspectorRegistrationActivityId };
                }
            }

        }

        public async Task<BuildingInspectorProfessionalBodyMembership> CreateOrUpdateBuildingInspectorProfessionalBodyMembership(BuildingInspectorProfessionalBodyMembership buildingInspectorProfessionalBodyMembership)
        {

            var dynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorProfessionalBodyMembership.BuildingInspectorId})",
                professionalBodyRefId: $"/accounts({buildingInspectorProfessionalBodyMembership.ProfessionalBodyId})",
                bsr_membershipnumber: buildingInspectorProfessionalBodyMembership.MembershipNumber,
                bsr_currentmembershiplevelortype: buildingInspectorProfessionalBodyMembership.CurrentMembershipLevelOrType,
                statuscode: buildingInspectorProfessionalBodyMembership.StatusCode ?? 1,
                statecode: buildingInspectorProfessionalBodyMembership.StateCode ?? 0,
                yearRefId: buildingInspectorProfessionalBodyMembership.YearId is null ? null : $"/bsr_years({buildingInspectorProfessionalBodyMembership.YearId})"
                );

            //If the membership has an id then we need to update it
            if (buildingInspectorProfessionalBodyMembership.Id is not null)
            {
                var response = await dynamicsApi.Update($"bsr_biprofessionalmemberships({buildingInspectorProfessionalBodyMembership.Id})", dynamicsBuildingInspectorProfessionalBodyMembership);
                var buildingInspectorRegistrationClassId = ExtractEntityIdFromHeader(response.Headers);
                return buildingInspectorProfessionalBodyMembership with { Id = buildingInspectorRegistrationClassId };
            }
            else
            {
                //Check if an entry for this membership already exists
                var existingProfessionalBodyMembership = await FindExistingBuildingProfessionalBodyMembership(buildingInspectorProfessionalBodyMembership.ProfessionalBodyId, buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId);

                //If no entry exists then create a new one
                if (existingProfessionalBodyMembership == null)
                {
                    var response = await dynamicsApi.Create("bsr_biprofessionalmemberships", dynamicsBuildingInspectorProfessionalBodyMembership);
                    var buildingInspectorProfessionalBodyMembershipId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorProfessionalBodyMembership with { Id = buildingInspectorProfessionalBodyMembershipId };
                }
                //If an entry exists then update it
                else
                {
                    var response = await dynamicsApi.Update($"bsr_biprofessionalmemberships({existingProfessionalBodyMembership.bsr_biprofessionalmembershipid})", dynamicsBuildingInspectorProfessionalBodyMembership);
                    var buildingInspectorProfessionalBodyMembershipId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorProfessionalBodyMembership with { Id = buildingInspectorProfessionalBodyMembershipId };
                }
            }

        }

        public async Task<BuildingInspectorEmploymentDetail> CreateOrUpdateBuildingInspectorEmploymentDetails(BuildingInspectorEmploymentDetail buildingInspectorEmploymentDetail)
        {

            var dynamicsBuildingInspectorEmploymentDetail = new DynamicsBuildingInspectorEmploymentDetail(
                buildingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorEmploymentDetail.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorEmploymentDetail.BuildingInspectorId})",
                employerIdContact: buildingInspectorEmploymentDetail.EmployerIdContact,
                employerIdAccount: buildingInspectorEmploymentDetail.EmployerIdAccount,
                bsr_iscurrent: buildingInspectorEmploymentDetail.IsCurrent,
                statuscode: buildingInspectorEmploymentDetail.StatusCode,
                statecode: buildingInspectorEmploymentDetail.StateCode,
                employmentTypeId: $"/bsr_employmenttypes({buildingInspectorEmploymentDetail.EmploymentTypeId})"
                );

            //If the employment detail has an id then we need to update it
            if (buildingInspectorEmploymentDetail.Id is not null)
            {
                var response = await dynamicsApi.Update($"bsr_biemploymentdetails({buildingInspectorEmploymentDetail.Id})", dynamicsBuildingInspectorEmploymentDetail);
                var buildingInspectorEmploymentDetailId = ExtractEntityIdFromHeader(response.Headers);
                return buildingInspectorEmploymentDetail with { Id = buildingInspectorEmploymentDetailId };
            }
            else
            {
                //Check if an entry for this demployment, already exists
                var existingRegistrationActivity = await GetEmploymentDetailsUsingId(buildingInspectorEmploymentDetail.BuildingProfessionApplicationId);

                //If no entry exists then create a new one
                if (existingRegistrationActivity == null)
                {

                    var response = await dynamicsApi.Create("bsr_biemploymentdetails", dynamicsBuildingInspectorEmploymentDetail);
                    var buildingInspectorEmploymentDetailId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorEmploymentDetail with { Id = buildingInspectorEmploymentDetailId };

                }
                //If an entry exists then update it
                else
                {
                    var response = await dynamicsApi.Update($"bsr_biemploymentdetails({existingRegistrationActivity.bsr_biemploymentdetailid})", dynamicsBuildingInspectorEmploymentDetail);
                    var buildingInspectorEmploymentDetailId = ExtractEntityIdFromHeader(response.Headers);
                    return buildingInspectorEmploymentDetail with { Id = buildingInspectorEmploymentDetailId };
                }
            }

        }


        public async Task<string[]> GetPublicSectorBodies()
        {
            var publicSectorBodies = await dynamicsApi.Get<DynamicsResponse<DynamicsAccount>>("accounts", ("$filter", $"_bsr_accounttype_accountid_value eq '{DynamicsAccountType.Ids["local-authority"]}'"));

            string[] publicSectorBodyNames = publicSectorBodies.value.Select(x => x.name).ToArray();

            return publicSectorBodyNames;
        }

        public async Task<DynamicsAccount> CreateOrUpdateEmployer(BuildingProfessionApplicationModel buildingProfessionApplicationModel)
        {
            var employer = await dynamicsApi.Get<DynamicsResponse<DynamicsAccount>>("accounts",
                                                           ("$filter", $"name eq '{buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerName.FullName}'" +
                                                           $" and address1_postalcode eq '{buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.Postcode}'"));
            var accountTypeId = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType switch
            {
                Enums.EmploymentType.PublicSector => DynamicsAccountType.Ids["public-sector-building-control-body"],
                Enums.EmploymentType.PrivateSector => DynamicsAccountType.Ids["private-sector-building-control-body"],
                _ => DynamicsAccountType.Ids["other"]
            };

            if (employer.value.Count() == 0)
            {
                
                CountryCodeMapper countryCodeMapper = new CountryCodeMapper();

                var country = countryCodeMapper.MapCountry(buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.Country);

                var countryId = countryCodeMapper.MapDynamicsCountryCode(country);

                //Create a new account
                var dynamicsAccount = new DynamicsAccount()
                {
                    name = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerName.FullName,
                    address1_addresstypecode = 3,
                    address1_line1 = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.Address,
                    address1_line2 = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.AddressLineTwo,
                    address1_city = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.Town,
                    address1_country = country,
                    address1_postalcode = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.Postcode,
                    bsr_address1uprn = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.UPRN,
                    bsr_address1usrn = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.USRN,
                    bsr_address1lacode = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.CustodianCode,
                    bsr_address1ladescription = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.CustodianDescription,
                    bsr_manualaddress = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.IsManual is null ? null : buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.IsManual is true ? YesNoOption.Yes : YesNoOption.No,
                    countryReferenceId = buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerAddress.Country is null ? null : $"/bsr_countries({countryId})"
                               
                };

                var response = await dynamicsApi.Create("accounts", dynamicsAccount);
                var employerId = ExtractEntityIdFromHeader(response.Headers);
                employer = await dynamicsApi.Get<DynamicsResponse<DynamicsAccount>>("accounts",
                                               ("$filter", $"accountid eq '{employerId}'"));

                //Update account type many to many relationship
                await AssignAccountType(employerId, accountTypeId);

                return employer.value.FirstOrDefault();
            }

            await AssignAccountType(employer.value.FirstOrDefault().accountid, accountTypeId);

            return employer.value.FirstOrDefault();
        }

        public Task<DynamicsOrganisationsSearchResponse> SearchSocialHousingOrganisations(string authorityName)
        {
            return SearchOrganisations(authorityName, DynamicsOptions.SocialHousingTypeId);
        }

        public Task<DynamicsOrganisationsSearchResponse> SearchOrganisations(string authorityName, string accountTypeId)
        {
            return dynamicsApi.Get<DynamicsOrganisationsSearchResponse>("accounts",
                new[] { ("$filter", $"_bsr_accounttype_accountid_value eq '{accountTypeId}' and contains(name, '{authorityName.EscapeSingleQuote()}')"), ("$select", "name") });
        }

        public Task<DynamicsOrganisationsSearchResponse> SearchLocalAuthorities(string authorityName)
        {
            return SearchOrganisations(authorityName, dynamicsOptions.LocalAuthorityTypeId);
        }

        public async Task<DynamicsYear> GetDynamicsYear(string year)
        {
            var dynamicsYear = await dynamicsApi.Get<DynamicsResponse<DynamicsYear>>("bsr_years", ("$filter", $"bsr_name eq '{year}'"));

            return dynamicsYear.value.FirstOrDefault();

        }

        public async Task<DynamicsBuildingInspectorEmploymentDetail> GetEmploymentDetailsUsingId(string applicationId)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingInspectorEmploymentDetail>>("bsr_biemploymentdetails", new[]
            {
            ("$filter", $"_bsr_biapplicationid_value eq '{applicationId}'")
            });
            return response.value.FirstOrDefault();
        }



        public async Task<bool> CheckDupelicateBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingProfessionApplicationModel)
        {
            if (featureOptions.DisableApplicationDuplicationCheck)
            {
                return false;
            }

            // Check for existing contacts
            var contact = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", new[]
            {
                ("$filter", $"firstname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.FirstName.EscapeSingleQuote()}' and lastname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.LastName.EscapeSingleQuote()}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantEmail.Email.EscapeSingleQuote()}'"),
            });

            foreach (var dynamicsContact in contact.value)
            {
                // Get all applications for contact which are not cancelled or inactive
                var existingApplications = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingProfessionApplication>>("bsr_buildingprofessionapplications", new[]
                {
                    ("$filter", $"_bsr_applicantid_value eq '{dynamicsContact.contactid}' and statuscode ne {(int)BuildingProfessionApplicationStatus.Cancelled} and statuscode ne {(int)BuildingProfessionApplicationStatus.Inactive} and bsr_buildingprofessiontypecode eq {(int)BuildingProfessionType.BuildingInspector}"),
                });

                if (existingApplications.value.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<DynamicsBuildingProfessionApplication> CheckDupelicateBuildingProfessionApplicationUsingCosmosIdAsync(string cosmosId)
        {
            var application = await dynamicsApi.Get<DynamicsResponse<DynamicsBuildingProfessionApplication>>("bsr_buildingprofessionapplications", new[]
                        {
                        ("$filter", $"bsr_cosmosid eq '{cosmosId}'"),
            });

            return application.value.FirstOrDefault();
        }
    }
}

