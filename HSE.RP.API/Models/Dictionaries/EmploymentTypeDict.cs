namespace HSE.RP.API.Models.Dictionarys
{
    public static class EmploymentTypeDict
    {
        public static string GetEmploymentType(string key)
        {
            IDictionary<string, string> employmentTypes = new Dictionary<string, string>
            {
                ["e5a761f1-0932-ee11-bdf3-0022481b56d1"] = "Public",
                ["f6d565f7-0932-ee11-bdf3-0022481b56d1"] = "Private",
                ["05d665f7-0932-ee11-bdf3-0022481b56d1"] = "Other",
                ["6a3f65fd-0932-ee11-bdf3-0022481b56d1"] = "Unemployed"
            };

            if (employmentTypes.ContainsKey(key))
            {
                return employmentTypes[key];
            }

            return string.Empty;
        }
    }
}
