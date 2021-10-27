using System;
using System.Collections.Generic;
using System.Text;

namespace FictionalOrg.ConfigurationProvider.Models
{
    public class ConfigProviderOptions
    {
        public string AWSAccessKey { get; set; }

        public string AWSSecretKey { get; set; }

        public string Environment { get; set; }

        public string ApplicationName { get; set; }

        public string Region { get; set; }

        public IDictionary<string,string> Configurationkeys { get; set; }
    }
}
