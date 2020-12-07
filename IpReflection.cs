using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ipMicroService
{
    public class IpReflection:IIpReflection
    {
        private IHttpContextAccessor _httpContextAccessor;
        public IpReflection(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetIp()
        {
            var headerPairs = _httpContextAccessor.HttpContext.Request.Headers;
            string XForIP = headerPairs["X-Forwarded-For"];
            if (XForIP == null)
                XForIP = "Null";
            return XForIP;
        }
    }
}
