using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace SimpleResponseFilter
{
    class FilterConfiguration
    {
        public List<string> GetUnsupportedHeaders(string siteName = "",  string configKey = "unsupportedHeaders")
        {
            var webConfig = string.IsNullOrWhiteSpace(siteName) ? WebConfigurationManager.OpenWebConfiguration("/appSettings") :
                                                                  WebConfigurationManager.OpenWebConfiguration("/appSettings", siteName);
            
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

        public List<string> GetUnsupportedHeadersOrDefault(string siteName = "", string configKey = "unsupportedHeaders")
        {
            // case: we have a site name to search within:
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                var results = GetUnsupportedHeaders(siteName, configKey); 

                if (results != null && results.Count != 0)
                {
                    return results;
                }
            }
            
            // fall-back/retry case: we have no site name, or nothing was found under sitename - search globally
            {
                var results = GetUnsupportedHeaders(string.Empty, configKey);

                if (results != null && results.Count != 0)
                {
                    return results;
                }
            }

            return new List<string> { "Server", "X-AspNet-Version", "X-AspNetMvc-Version", "X-Powered-By" }; // default;
        }
    }
}
