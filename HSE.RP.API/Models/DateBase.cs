using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record DateBase
	{
		public required string Day { get; set; }
        public required string Month { get; set; }
        public required string Year { get; set; }

    }
}

