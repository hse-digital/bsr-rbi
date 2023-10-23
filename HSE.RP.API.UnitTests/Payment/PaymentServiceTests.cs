using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HSE.RP.API.Functions;
using HSE.RP.API.Mappers;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask;
using Microsoft.Extensions.Options;
using Moq;
using HSE.RP.API.UnitTests.Helpers;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;
using HSE.RP.API.Models.Payment.Response;
using Flurl.Http.Testing;
using Xunit;
using HSE.RP.Domain.Entities;
using HSE.RP.API.Models.Payment;
using Castle.Core.Resource;
using Grpc.Core;
using System.Text.RegularExpressions;
using BuildingInspectorClass = HSE.RP.API.Models.BuildingInspectorClass;

namespace HSE.RP.API.UnitTests.Payment
{
    public class PaymentServiceTests: UnitTestBase
    {
        public class TestHarness
        {
            public Mock<IDynamicsService> DynamicsService = new Mock<IDynamicsService>();
            public Mock<DurableTaskClient> durableTaskClient = new Mock<DurableTaskClient>("DurableTaskClientName");
            public Mock<TaskOrchestrationContext> taskOrchestrationContext = new();
            public Mock<IMapper> Mapper = new();
            public Mock<IPaymentService> paymentService = new();
            public Mock<IPaymentMapper> paymentMapper = new();
            public HttpTest HttpTest = new HttpTest();

            public IntegrationsOptions IntegrationsOptions = new IntegrationsOptions()
            {
                CommonAPIEndpoint = "http://localhost:7126",
                CommonAPIKey = "",
                PaymentAmount=99999,
                Environment="dev"

            };

            public PaymentService SUT()
            {
                return new PaymentService(new OptionsWrapper<IntegrationsOptions>(IntegrationsOptions), new OptionsWrapper<SwaOptions>(new SwaOptions()), DynamicsService.Object, paymentMapper.Object);
            }
            public PaymentRequestModel BuildPaymentRequestModel(BuildingProfessionApplicationModel model)
            {
                var reference = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..22], @"\W", "0");

                var paymentRequestModel = new PaymentRequestModel
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
                return paymentRequestModel;
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

        public PaymentServiceTests()
        {

        }

        public BuildingProfessionApplicationModel Application = new BuildingProfessionApplicationModel()
        {
            Id = "ABC123",
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


        [Fact]
        public async Task ShouldCreateCardPayment()
        {
            //Arrange
            TestHarness testHarness = new TestHarness();
            var sut = testHarness.SUT();
            var paymentRequestModel = testHarness.BuildPaymentRequestModel(Application);
            var responseModel = new PaymentResponseModel
            {
                AddressLineOne = Application.PersonalDetails.ApplicantAddress.Address,
                AddressLineTwo = Application.PersonalDetails.ApplicantAddress.AddressLineTwo,
                City = Application.PersonalDetails.ApplicantAddress.Town,
                Amount = 9999,
                Reference = paymentRequestModel.Reference,
                ReturnURL = paymentRequestModel.ReturnUrl,

            };
            var newPayment = new NewCardPaymentRequest
            {
                Reference = paymentRequestModel.Reference,
                Amount = 9999,
                ReturnUrl = paymentRequestModel.ReturnUrl,
                CardHolder = new GovukPaymentCardHolderDetails
                {
                    Name = paymentRequestModel.CardHolderDetails.Name,
                    Address = new GovukPaymentCardHolderAddress
                    {
                        Line1 = paymentRequestModel.CardHolderDetails.Address.Line1,
                        Line2 = paymentRequestModel.CardHolderDetails.Address.Line2,
                        Postcode = paymentRequestModel.CardHolderDetails.Address.Postcode,
                        City = paymentRequestModel.CardHolderDetails.Address.City
                    }
                }

            };

            testHarness.HttpTest.RespondWithJson(responseModel);

            await sut.CreateCardPayment(newPayment);


            testHarness.HttpTest.ShouldHaveCalled($"{testHarness.IntegrationsOptions.CommonAPIEndpoint}/api/CreateCardPayment")
                               .WithRequestJson(newPayment).WithVerb(HttpMethod.Post);
        }


        [Fact]
        public async Task ShouldCreateNewInvoicePayment()
        {
            TestHarness testHarness = new TestHarness();





            var paymentModel = new NewInvoicePaymentRequestModel
            {
                Name = Application.PersonalDetails.ApplicantName.FirstName + ' ' + Application.PersonalDetails.ApplicantName.LastName,
                Email = Application.PersonalDetails.ApplicantEmail.Email,
                OrderNumber = "124",
                AddressLine1 = Application.PersonalDetails.ApplicantAddress.Address,
                AddressLine2 = Application.PersonalDetails.ApplicantAddress.AddressLineTwo,
                Postcode = Application.PersonalDetails.ApplicantAddress.Postcode,
                Town = Application.PersonalDetails.ApplicantAddress.Town
            };
            var dynamicContact = new DynamicsContact
            {
                contactid="1"
            };
            var payment = new DynamicsPayment
            {
                bsr_paymentid="1"
            };
            
            var invoiceReq = new CreateInvoiceRequest
            {
                Amount = Math.Round((float)testHarness.IntegrationsOptions.PaymentAmount / 100, 2),
                PaymentId = payment.bsr_paymentid,
                Name = paymentModel.Name,
                Email = paymentModel.Email,
                AddressLine1 = paymentModel.AddressLine1,
                AddressLine2 = paymentModel.AddressLine2,
                Town = paymentModel.Town,
                Postcode = paymentModel.Postcode,
                Application = "RBI",
                Description = $"Building Professional Application: {Application.Id}",
                Title = "RBI",
                OrderNumber = paymentModel.OrderNumber,
                CustomerId = dynamicContact.contactid.ToUpper(),
                Environment = testHarness.IntegrationsOptions.Environment
            };

            var invoicedata = new InvoiceData
            {
                //InvoiceId = "1",
                AccountCountry="GB",
                AccountName= "HSE as the Building Safety Regulator",
                AmountDue= 99999,
                AmountPaid=  0   ,
                AmountRemaining=99999   ,
                AttemptCount=    0   ,
                CreatedDate =1694188883  ,
                CustomerId = "EAEC5EFB-4010-EE11-8F6E-6045BDD0E4CF"  ,
                Description= "Building Professional Application: BCA01126Y7L8\nOrder Number: 2243453",                
                InvoiceId =  "in_1No77XBVm8oE35l48hmwiMhe"   ,
                InvoiceMetadata  = new InvoiceMetadata{
                    PaymentId = payment.bsr_paymentid,
                    Environment = testHarness.IntegrationsOptions.Environment,
                    Application= "RBI"
                },
                InvoiceNumber =  "4B9436C2-0083",
                Paid  =  false  ,
                Status = "open" 
            };




            testHarness.HttpTest.ForCallsTo("http://localhost:7126/api/CreateInvoice").RespondWithJson(invoicedata, 200);

            var sut = testHarness.SUT();
            //var requestData = testHarness.BuildHttpRequestDataWithUri(paymentModel, new Uri("http://noaddress.com"));

            testHarness.DynamicsService.Setup(X => X.GetOrCreateInvoiceContactAsync(paymentModel)).Returns(Task.FromResult(dynamicContact));
            testHarness.DynamicsService.Setup(x => x.GetBuildingProfessionApplicationUsingId(Application.Id));
            testHarness.paymentMapper.Setup(x => x.ToDynamics(Application.Id, dynamicContact, paymentModel)).Returns(payment);
            testHarness.DynamicsService.Setup(x => x.CreatePaymentAsync(payment, Application.Id)).Returns(Task.FromResult(payment));
            testHarness.paymentMapper.Setup(x => x.ToDynamics(payment.bsr_paymentid, invoicedata)).Returns(payment);
            testHarness.DynamicsService.Setup(x => x.SendCreateInvoiceRequest(IntegrationsOptions.Value, invoiceReq)).Returns(Task.FromResult(invoicedata));


            await sut.NewInvoicePayment(Application, paymentModel);




            testHarness.DynamicsService.Verify(x => x.SendCreateInvoiceRequest(It.IsAny<IntegrationsOptions>(), It.IsAny<CreateInvoiceRequest>()), Times.Once);


        }



    }
}
