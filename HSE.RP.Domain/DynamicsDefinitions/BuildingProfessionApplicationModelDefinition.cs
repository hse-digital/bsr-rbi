using HSE.RP.Domain.Entities;

namespace HSE.RP.Domain.DynamicsDefinitions;

public class BuildingProfessionApplicationModelDefinition : DynamicsModelDefinition<BuildingProfessionApplication, DynamicsBuildingProfessionApplication>
{
    public override string Endpoint => "bsr_buildingprofessionapplications";

    public override DynamicsBuildingProfessionApplication BuildDynamicsEntity(BuildingProfessionApplication entity)
    {
        return new DynamicsBuildingProfessionApplication(bsr_applicantid: $"/contacts({entity.ContactId})", bsr_buildingprofessiontypecode: entity.BuildingProfessionTypeCode, bsr_buildingprofessionapplicationid: entity.Id);
    }

    public override BuildingProfessionApplication BuildEntity(DynamicsBuildingProfessionApplication dynamicsEntity)
    {
        return new BuildingProfessionApplication(dynamicsEntity.bsr_applicantid, ApplicationReturnId: dynamicsEntity.bsr_buildingproappid, BuildingProfessionTypeCode: dynamicsEntity.bsr_buildingprofessiontypecode);
    }
}   