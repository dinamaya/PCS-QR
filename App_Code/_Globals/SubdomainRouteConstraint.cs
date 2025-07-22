using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CCIMS.Web.App_Code._Globals
{
    public class SubdomainRouteConstraint : IRouteConstraint
    {
        private readonly string[] _subdomains;

        public SubdomainRouteConstraint(params string[] subdomains)
        {
            _subdomains = subdomains;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var host = httpContext.Request.Host.Host;
            var parts = host.Split('.');
            if (parts.Length < 2)
            {
                return false;
            }
            var subdomain = parts[0];
            return _subdomains.Contains(subdomain);
        }
    }
}
