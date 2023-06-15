using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models
{
    public interface IValidatableModel
    {
        ValidationSummary Validate();
    }
    public record ValidationSummary(bool IsValid, string[] Errors);
}
