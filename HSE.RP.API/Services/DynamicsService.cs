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
            var contact = new Contact(/*FirstName: model.PersonalDetails.ApplicantName.FirstName ?? "",
                                      LastName: model.PersonalDetails.ApplicantName.LastName ?? "",
                                      PhoneNumber: model.PersonalDetails.ApplicantPhone ?? "",*/ //TODO Remove comments as fields become required
                                      Email: model.PersonalDetails.ApplicantEmail,
                                      jobRoleReferenceId: $"/bsr_jobroles({DynamicsJobRole.Ids["building_inspector"]})" 
                                      ); ;
            var dynamicsContact = modelDefinition.BuildDynamicsEntity(contact);

            var existingContact = await FindExistingContactAsync(/*contact.FirstName, contact.LastName, */contact.Email); //TODO add back phone number in next sprint
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


        private async Task<DynamicsContact> FindExistingContactAsync(/*string firstName, string lastName, */string email)
        {
            /*            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", new[]
                        {
                        ("$filter", $"firstname eq '{firstName.EscapeSingleQuote()}' and lastname eq '{lastName.EscapeSingleQuote()}' and emailaddress1 eq '{email.EscapeSingleQuote()}' and contains(telephone1, '{phoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')"),
                        ("$expand", "bsr_contacttype_contact") //TODO add back phone in next sprint

                    });*/

            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", new[]
{
                        ("$filter", $"emailaddress1 eq '{email.EscapeSingleQuote()}'"),
                        ("$expand", "bsr_contacttype_contact") //TODO add back phone in next sprint

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

    }


}

