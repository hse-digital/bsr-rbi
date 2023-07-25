using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ClassSelection
    (
       BuildingInspectorClassType Class = BuildingInspectorClassType.ClassNone,
       ComponentCompletionState CompletionState = ComponentCompletionState.NotStarted
    );
}

