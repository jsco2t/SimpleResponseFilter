using System;
using System.Collections.Generic;
using System.Web;

namespace SimpleResponseHeaderFilterModule
{
    public class FilterModule : IHttpModule
    {
        private static List<string> unsupportedHeaders = null; 
         
        public void Init(HttpApplication context)
        {
            if (context != null)
            {
                context.PreSendRequestHeaders += OnPreSendRequestHeaders;
            }

            if (unsupportedHeaders == null)
            {
                FilterConfiguration config = new FilterConfiguration();
                unsupportedHeaders = config.GetUnsupportedHeaders();

                if (unsupportedHeaders == null || unsupportedHeaders.Count == 0)
                {
                    // default set
                    unsupportedHeaders = new List<string> { "Server", "X-AspNet-Version", "X-AspNetMvc-Version", "X-Powered-By" };
                }
            }
        }
        
        public void Dispose()
        {
            
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender != null ? (HttpApplication)sender : null;
            var response = app != null ? app.Response : null;

            if (response != null)
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
// http://forums.iis.net/t/1151098.aspx?How+to+read+IIS+7+metabase+from+HttpModule+C+
