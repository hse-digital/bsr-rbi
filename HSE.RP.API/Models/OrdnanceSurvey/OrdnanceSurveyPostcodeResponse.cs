namespace HSE.RP.API.Models.OrdnanceSurvey;

public class OrdnanceSurveyPostcodeResponse
{
    public Header header { get; set; }
    public List<Result> results { get; set; }
}

public class Result
{
    public LPI LPI { get; set; }
    public DPA DPA { get; set; }
}

public class Header
{
    public int offset { get; set; }
    public int totalresults { get; set; }
    public int maxresults { get; set; }
}

public class LPI : BASEADDRESS
{
    public string LPI_KEY { get; set; }
    public string PAO_START_NUMBER { get; set; }
    public string PAO_TEXT { get; set; }
    public string STREET_DESCRIPTION { get; set; }
    public string TOWN_NAME { get; set; }
    public string ADMINISTRATIVE_AREA { get; set; }
    public string POSTCODE_LOCATOR { get; set; }
    public string LPI_LOGICAL_STATUS_CODE { get; set; }
    public string LPI_LOGICAL_STATUS_CODE_DESCRIPTION { get; set; }
}

public class DPA : BASEADDRESS
{
    public string UDPRN { get; set; }
    public string SUB_BUILDING_NAME { get; set; }
    public string BUILDING_NUMBER { get; set; }
    public string THOROUGHFARE_NAME { get; set; }
    public string POST_TOWN { get; set; }
    public string POSTCODE { get; set; }
    public string PARENT_UPRN { get; set; }
    public string DELIVERY_POINT_SUFFIX { get; set; }
}

public class BASEADDRESS
{
    public string USRN { get; set; }
    public string UPRN { get; set; }
    public string ADDRESS { get; set; }
    public string LAST_UPDATE_DATE { get; set; }
    public string ENTRY_DATE { get; set; }
    public string BLPU_STATE_DATE { get; set; }
    public string LANGUAGE { get; set; }
    public double MATCH { get; set; }
    public string MATCH_DESCRIPTION { get; set; }
    public string STATUS { get; set; }
    public string LOGICAL_STATUS_CODE { get; set; }
    public string CLASSIFICATION_CODE { get; set; }
    public string CLASSIFICATION_CODE_DESCRIPTION { get; set; }
    public int LOCAL_CUSTODIAN_CODE { get; set; }
    public string LOCAL_CUSTODIAN_CODE_DESCRIPTION { get; set; }
    public string COUNTRY_CODE { get; set; }
    public string COUNTRY_CODE_DESCRIPTION { get; set; }
    public string POSTAL_ADDRESS_CODE { get; set; }
    public string POSTAL_ADDRESS_CODE_DESCRIPTION { get; set; }
    public string BLPU_STATE_CODE { get; set; }
    public string BLPU_STATE_CODE_DESCRIPTION { get; set; }
    public string TOPOGRAPHY_LAYER_TOID { get; set; }
    public string RPC { get; set; }
    public double X_COORDINATE { get; set; }
    public double Y_COORDINATE { get; set; }
}