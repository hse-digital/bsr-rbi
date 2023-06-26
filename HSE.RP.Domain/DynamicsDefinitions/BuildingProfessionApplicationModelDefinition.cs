using HSE.RP.Domain.Entities;

namespace HSE.RP.Domain.DynamicsDefinitions;

public class BuildingProfessionApplicationModelDefinition : DynamicsModelDefinition<BuildingProfessionApplication, DynamicsBuildingProfessionApplication>
{
    public override string Endpoint => "bsr_buildingprofessionapplications";

    public override DynamicsBuildingProfessionApplication BuildDynamicsEntity(BuildingProfessionApplication entity)
    {
        return new DynamicsBuildingProfessionApplication(contactReferenceId: $"/contacts({entity.Id})", bsr_buildingprofessiontypecode: DynamicsBuildingProfessionTypeCode.Ids["Building Inspector"] );
    }

    public override BuildingProfessionApplication BuildEntity(DynamicsBuildingProfessionApplication dynamicsEntity)
    {
        return new BuildingProfessionApplication(dynamicsEntity.contactReferenceId, dynamicsEntity.bsr_buildingproappid);
    }
}