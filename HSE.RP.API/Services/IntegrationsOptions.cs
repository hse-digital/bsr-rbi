namespace HSE.RP.API.Services;

public class IntegrationsOptions
{
    public const string Integrations = "Integrations";
    
    public string NotificationServiceApiKey { get; set; }
    public string NotificationServiceOTPEmailTemplateId { get; set; }
    public string NotificationServiceReplyToId { get; set; }

}