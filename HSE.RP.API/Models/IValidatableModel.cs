namespace HSE.RP.API.Models
{
    public interface IValidatableModel
    {
        ValidationSummary Validate();
    }
    public record ValidationSummary(bool IsValid, string[] Errors);
}
