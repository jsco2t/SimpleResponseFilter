using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Hosting;

namespace SimpleResponseFilter
{
    public class FilterModule : IHttpModule
    {
        private static List<string> unsupportedHeaders = null;

        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                return; // nothing for us to do / unexpected condition
            }
            
            if (unsupportedHeaders == null)
            {
                try
                {
                    FilterConfiguration config = new FilterConfiguration();
                    string siteName = !string.IsNullOrWhiteSpace(HostingEnvironment.SiteName) ? HostingEnvironment.SiteName : string.Empty;

                    unsupportedHeaders = config.GetUnsupportedHeadersOrDefault(siteName);
                }
                catch
                { } // do nothing - todo add logging
            }

            if (unsupportedHeaders != null && unsupportedHeaders.Count != 0)
            {
                // only wire up the event handler - if we have filtering to do:
                context.PreSendRequestHeaders += OnPreSendRequestHeaders;
            }
        }

        public void Dispose()
        {
            // required by IHttpModule interface
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender != null ? sender as HttpApplication : null;
            var response = app != null ? app.Response : null;

            if (response != null && unsupportedHeaders != null)
            {
                unsupportedHeaders.ForEach(header => response.Headers.Remove(header));
            }
        }
    }
}

// references:
// http://www.iis.net/learn/get-started/introduction-to-iis/iis-modules-overview
// http://www.iis.net/learn/develop/runtime-extensibility/developing-iis-modules-and-handlers-with-the-net-framework
// https://msdn.microsoft.com/en-us/library/dkkx7f79(v=vs.110).aspx <-- gacutil
