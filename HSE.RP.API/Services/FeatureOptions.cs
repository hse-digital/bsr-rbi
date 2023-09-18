﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Services
{
    public class FeatureOptions
    {

        public const string Feature = "Feature";

        public bool DisableOtpValidation { get; set; }
        public bool DisableApplicationDuplicationCheck { get; set; }
    }
}
