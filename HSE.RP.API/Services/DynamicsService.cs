using System;
using System.Collections.Generic;
using System.Linq;
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

        public DynamicsService(IOptions<DynamicsOptions> dynamicsOptions, IOptions<SwaOptions> swaOptions)
        {
            this.dynamicsOptions = dynamicsOptions.Value;
            this.swaOptions = swaOptions.Value;
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
            var buildingProfessionApplication = await CreateBuildingProfessionApplicationAsync(buildingProfessionApplicationModel, contact);

            return buildingProfessionApplicationModel with { Id = buildingProfessionApplication.Id };
        }

        private Task CreateBuildingInspectorApplicationAsync(object contact, object application)
        {
            throw new NotImplementedException();
        }

        private async Task<Contact> CreateContactAsync(BuildingProfessionApplicationModel model)
        {
            var modelDefinition = dynamicsModelDefinitionFactory.GetDefinitionFor<Contact, DynamicsContact>();
            var contact = new Contact(model.PersonDetails.ApplicantName.FirstName,
                                      model.PersonDetails.ApplicantName.LastName, 
                                      model.PersonDetails.ApplicantPhone, 
                                      model.PersonDetails.ApplicantEmail);
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

        private async Task<DynamicsContact> FindExistingContactAsync(string firstName, string lastName, string email, string phoneNumber)
        {
            var response = await dynamicsApi.Get<DynamicsResponse<DynamicsContact>>("contacts", new[]
            {
            ("$filter", $"firstname eq '{firstName.EscapeSingleQuote()}' and lastname eq '{lastName.EscapeSingleQuote()}' and emailaddress1 eq '{email.EscapeSingleQuote()}' and contains(telephone1, '{phoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')"),
            ("$expand", "bsr_contacttype_contact")
        });

            return response.value.FirstOrDefault();
        }

        private async Task<BuildingProfessionApplicationModel> CreateBuildingProfessionApplicationAsync(BuildingProfessionApplicationModel buildingInspectorModel, Contact contact)
        {
            var modelDefinition = dynamicsModelDefinitionFactory.GetDefinitionFor<BuildingProfessionApplication, DynamicsBuildingProfessionApplication>();
            var buildingProfessional = new BuildingProfessionApplication(contact.Id);
            var dynamicsbuildingProfessional = modelDefinition.BuildDynamicsEntity(buildingProfessional);

            var response = await dynamicsApi.Create(modelDefinition.Endpoint, dynamicsbuildingProfessional);
            var buildingProfessionalApplicationId = ExtractEntityIdFromHeader(response.Headers);
            return buildingInspectorModel with { Id = buildingProfessionalApplicationId };
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

