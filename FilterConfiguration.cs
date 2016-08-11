using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace SimpleResponseFilter
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
                            else
                            {
                                // if key exists - but value is empty return no unsupported headers:
                                return new List<string>();
                            }
                        }
                    }
                }
            }

            return new List<string> { "Server", "X-AspNet-Version", "X-AspNetMvc-Version", "X-Powered-By" }; // default
        }
    }
}
