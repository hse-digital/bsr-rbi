using HSE.RP.API.Enums;
using HSE.RP.Domain.Entities;

namespace HSE.RP.API.Models
{
    public class AddressModel : BuildingAddress
    {
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;

    }
}


