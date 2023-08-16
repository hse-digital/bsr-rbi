using HSE.RP.API.Enums;
using HSE.RP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models
{
    public class AddressModel : BuildingAddress
    {
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;

    }
}


