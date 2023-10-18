using HSE.RP.API.Enums;
namespace HSE.RP.API.Models
{
    public record BuildingInspectorCountryOfWork
    {
        public bool? England { get; set; }
        public bool? Wales { get; set; }
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;

    }
}

