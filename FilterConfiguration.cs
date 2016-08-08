using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace ResponseHeadersFilterModule
{
    class FilterConfiguration
    {
        public List<string> GetUnsupportedHeaders(string configKey = "unsupportedHeaders")
        {
            var webConfig = WebConfigurationManager.OpenWebConfiguration("/appSettings");
            
            if (webConfig != null)
            {
                if (webConfig.AppSettings != null && webConfig.AppSettings.Settings != null)
                {
                    foreach (KeyValueConfigurationElement kvp in webConfig.AppSettings.Settings)
                    {
                        if (string.Equals(kvp.Key, configKey))
                        {
                            if (!string.IsNullOrWhiteSpace(kvp.Value))
                            {
                                if (kvp.Value.Contains(","))
                                {
                                    return new List<string>(kvp.Value.Split(','));
                                }
                                else
                                {
                                    return new List<string>() { kvp.Value };
                                }
                            }
                        }
                    }
                }
            }

            return new List<string>();
        }
    }
}
