using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HSE.RP.API.Functions;
using HSE.RP.API.Mappers;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask;
using Microsoft.Extensions.Options;
using Moq;
using HSE.RP.API.Models.Payment.Request;
using static System.Net.Mime.MediaTypeNames;
using HSE.RP.API.UnitTests.Helpers;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.Domain.Entities;
using Xunit;
using HSE.RP.API.Models.Payment;
using HSE.RP.API.Extensions;
using BuildingInspectorClass = HSE.RP.API.Models.BuildingInspectorClass;
using System.Text.RegularExpressions;

namespace HSE.RP.API.UnitTests.Payment
{
    public class PaymentFuncationTest
    {
        public BuildingProfessionApplicationModel Application = new BuildingProfessionApplicationModel()
        {
            Id = "ABC123",
            CosmosId = "ABC123",
            PersonalDetails = new PersonalDetails
            {
                ApplicantName = new ApplicantName
                {
                    FirstName = "John",
                    LastName = "Doe"
                },
                ApplicantEmail = new ApplicantEmail
                {
                    Email = "jdoe@codec.ie"
                },
                ApplicantAddress = new AddressModel { Address = "abc", AddressLineTwo = "123", Town = "London", Postcode = "DC13H32" },
            },
            InspectorClass = new BuildingInspectorClass(),
            Competency = new Competency(),
            ProfessionalActivity = new ProfessionalActivity(),
            ProfessionalMemberships = new ApplicantProfessionBodyMemberships()
        };

        public DynamicsBuildingProfessionApplication DynamicsApplication = new DynamicsBuildingProfessionApplication()
        {
            bsr_buildingprofessionapplicationid = "1"
        };


        public class TestHarness
        {
            public Mock<IDynamicsService> DynamicsService = new Mock<IDynamicsService>();
            public Mock<DurableTaskClient> durableTaskClient = new Mock<DurableTaskClient>("DurableTaskClientName");
            public Mock<TaskOrchestrationContext> taskOrchestrationContext = new();
            public Mock<IOptions<IntegrationsOptions>> IntegrationOptions = new();
            public Mock<IMapper> Mapper = new();
            public Mock<IPaymentService> paymentService = new();
            public Mock<IPaymentMapper> paymentMapper = new();
            public Mock<IOptions<SwaOptions>> swaOption = new();

            public PaymentFunctions SUT()
            {
                return new PaymentFunctions(IntegrationOptions.Object, swaOption.Object, DynamicsService.Object, Mapper.Object, paymentService.Object, paymentMapper.Object);
            }
            public PaymentRequestModel BuildPaymentRequestModel(BuildingProfessionApplicationModel model)
            {
                var reference = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..22], @"\W", "0");
                return new PaymentRequestModel
                {
                    Reference = reference,
                    ReturnUrl = $"www.google.com/app/${model.Id}/payment/confirm",
                    Email = model.PersonalDetails.ApplicantEmail.Email,
                    CardHolderDetails = new CardHolderDetails
                    {
                        Name = model.PersonalDetails.ApplicantName.FirstName + " " + model.PersonalDetails.ApplicantName.LastName,
                        Address = new CardHolderAddress
                        {
                            Line1 = model.PersonalDetails.ApplicantAddress.Address,
                            Line2 = model.PersonalDetails.ApplicantAddress.AddressLineTwo,
                            Postcode = model.PersonalDetails.ApplicantAddress.Postcode,
                            City = model.PersonalDetails.ApplicantAddress.Town,
                        }
                    }
                };
            }
            public TestableHttpRequestData BuildHttpRequestDataWithUri<T>(T data, Uri uri)
            {
                var functionContext = new Mock<FunctionContext>();

                var memoryStream = new MemoryStream();
                JsonSerializer.Serialize(memoryStream, data);

                memoryStream.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return new TestableHttpRequestData(functionContext.Object, uri, memoryStream);
            }

        }
        protected EncodedRequest Base64Encode(Object plainObject)
        {
            var plainText = JsonSerializer.Serialize(plainObject);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return new EncodedRequest("base64:" + System.Convert.ToBase64String(plainTextBytes));
        }

        [Fact]
        public async Task ShouldInitialisePayment()
        {
            //Arrange
            TestHarness testHarness = new TestHarness();
            var paymentRequestModel = testHarness.BuildPaymentRequestModel(Application);
            var sut = testHarness.SUT();
            var requestData = testHarness.BuildHttpRequestDataWithUri(Application, new Uri("http://noaddress.com"));

            var newPayment = new NewCardPaymentRequest
            {
                Reference = paymentRequestModel.Reference,
                Amount = 9999,
                ReturnUrl = paymentRequestModel.ReturnUrl,
                CardHolder = new GovukPaymentCardHolderDetails
                {
                    Name =  paymentRequestModel.CardHolderDetails.Name, 
                    Address =  new GovukPaymentCardHolderAddress
                    {
                       Line1= paymentRequestModel.CardHolderDetails.Address.Line1,
                       Line2= paymentRequestModel.CardHolderDetails.Address.Line2,
                       Postcode= paymentRequestModel.CardHolderDetails.Address.Postcode,
                       City= paymentRequestModel.CardHolderDetails.Address.City
                    }
                }

                };
            var responseModel = new PaymentResponseModel
            {
                AddressLineOne = Application.PersonalDetails.ApplicantAddress.Address,
                AddressLineTwo = Application.PersonalDetails.ApplicantAddress.AddressLineTwo,
                City = Application.PersonalDetails.ApplicantAddress.Town,
                Amount = 9999,
                Reference = paymentRequestModel.Reference,
                ReturnURL = paymentRequestModel.ReturnUrl,

            };
            var dynamicsPayment = new DynamicsPayment
            {

                bsr_transactionid = responseModel.Reference,
                bsr_paymenttypecode = 760_810_000,
                bsr_service = "RBI",
                bsr_billingaddress = string.Join(", ", new[] { responseModel.AddressLineOne, responseModel.AddressLineTwo, responseModel.Postcode, responseModel.City, responseModel.Country }.Where(x => !string.IsNullOrWhiteSpace(x))),
                bsr_amountpaid = Math.Round((float)responseModel.Amount / 100, 2),
            };

            testHarness.paymentService.Setup(c => c.BuildPaymentRequest(Application)).Returns(newPayment);
            testHarness.paymentService.Setup(c => c.CreateCardPayment(newPayment)).Returns(Task.FromResult( responseModel));
            testHarness.paymentMapper.Setup(c => c.ToDynamics(responseModel)).Returns(dynamicsPayment);
            testHarness.DynamicsService.Setup(x => x.CreatePaymentAsync(dynamicsPayment, DynamicsApplication.bsr_buildingprofessionapplicationid)).Returns(Task.FromResult(dynamicsPayment with { bsr_paymentid="iujhsfdio809" }));
            testHarness.DynamicsService.Setup(x => x.GetBuildingProfessionApplicationUsingId(Application.Id)).Returns(Task.FromResult(DynamicsApplication));


            //act
            await sut.InitialisePayment(requestData, Application);

            //assert
            testHarness.paymentService.Verify(x => x.CreateCardPayment(newPayment), Times.Once());
            testHarness.DynamicsService.Verify(x => x.CreatePaymentAsync(dynamicsPayment, DynamicsApplication.bsr_buildingprofessionapplicationid), Times.Once());

        }



        [Fact]
        public async Task ShouldInitialiseInvoice()
        {
            TestHarness testHarness = new TestHarness();
            var paymentModel = new NewInvoicePaymentRequestModel
            {
                Name = Application.PersonalDetails.ApplicantName.FirstName+' '+ Application.PersonalDetails.ApplicantName.LastName,
                Email = Application.PersonalDetails.ApplicantEmail.Email,
                OrderNumber = "124",
                AddressLine1 = Application.PersonalDetails.ApplicantAddress.Address,
                AddressLine2 = Application.PersonalDetails.ApplicantAddress.AddressLineTwo,
                Postcode = Application.PersonalDetails.ApplicantAddress.Postcode,
                Town = Application.PersonalDetails.ApplicantAddress.Town
            };
            var sut = testHarness.SUT();
            var requestData = testHarness.BuildHttpRequestDataWithUri(paymentModel, new Uri("http://noaddress.com"));

            var endcoded = Base64Encode(paymentModel);
            await sut.InitialiseInvoicePayment(requestData, endcoded, Application);


            testHarness.paymentService.Verify(x => 
            x.NewInvoicePayment(It.Is<BuildingProfessionApplicationModel>(m => m.Id == Application.Id),
            It.Is<NewInvoicePaymentRequestModel>(m => m.OrderNumber == paymentModel.OrderNumber))
            , Times.Once);
        }

    }
}
