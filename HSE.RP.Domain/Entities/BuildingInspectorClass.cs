using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorClass(
    string Id = null,
    BuildingProfessionApplication Application = null,
    string Status = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorClass(
    string bsr_biclassid = null,
    string bsr_buildingprofessionapplicationid = null,
    string statuscode = null
) : DynamicsEntity<BuildingInspectorClass>;