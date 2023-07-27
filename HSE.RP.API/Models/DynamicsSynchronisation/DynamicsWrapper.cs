using HSE.RP.API.Models.Payment.Response;
using HSE.RP.Domain.Entities;

namespace HSE.RP.API.Models.DynamicsSynchronisation;

public record BuildingProfessionApplicationWrapper(BuildingProfessionApplicationModel Model, DynamicsBuildingProfessionApplication DynamicsBuildingProfessionApplication/*, ApplicationStatus Stage*/);

public record BuildingProfessionApplicationPayment(string BuildingProfessionApplicationId, PaymentResponseModel Payment);

public record ContactWrapper(Contact Model, DynamicsContact DynamicsContact);

