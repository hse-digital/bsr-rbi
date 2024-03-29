namespace HSE.RP.API.Services;

public class IntegrationsOptions
{
    public const string Integrations = "Integrations";

    public string NotificationsAPIEndpoint { get; set; }
    public string NotificationServiceApiKey { get; set; }
    public string NotificationServiceOTPEmailTemplateId { get; set; }
    public string NotificationServiceOTPSmsTemplateId { get; set; }
    public string NotificationServiceReplyToId { get; set; }
    public string PaymentEndpoint { get; set; }
    public string PaymentApiKey { get; set; }
    public double PaymentAmount { get; set; }

    public string CommonAPIEndpoint { get; set; }
    public string CommonAPIKey { get; set; }
    public string CompaniesHouseEndpoint { get; set; }
    public string CompaniesHouseApiKey { get; set; }

    public string Environment { get; set; }

    public string CosmosConnection { get; set; }
    public string CosmosDatabase { get; set; }
    public string CosmosContainer { get; set; }


}