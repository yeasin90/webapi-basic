using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace tutWebApi1.Services
{
    public class CustomWebApiControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;

        public CustomWebApiControllerSelector(HttpConfiguration config) : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping();

            var routeData = request.GetRouteData();

            var controllerName = (string)routeData.Values["controller"];

            HttpControllerDescriptor descriptor;

            if (controllers.TryGetValue(controllerName, out descriptor))
            {
                // get version using WepApi.Config routing

                // hard coded version
                // var version = "2";

                // out request string in Fiddler would be
                // http://localhost/api/nutrition/foods/123/measures/234?v=2
                // now if v is ommited, then we would select v = 1 by default
                // var version = GetVersionFromQueryString(request);

                // version from request header
                // request http header :
                // X-webApi1Tut-Version: 2
                //var version = GetVersionFromHeader(request);

                // version from Accept header = Accept : application/json; version=2
                var version = GetVersionFromAcceptHeaderVersion(request);

                // version from Media Types
                //var version = GetVersionFromMediaType(request);

                var newName = string.Concat(controllerName, "V", version);

                HttpControllerDescriptor versionedDescriptor;

                if (controllers.TryGetValue(newName, out versionedDescriptor))
                {
                    return versionedDescriptor;
                }

                return descriptor;
            }

            return null;
        }

        private string GetVersionFromAcceptHeaderVersion(HttpRequestMessage request)
        {
            var accept = request.Headers.Accept;

            foreach (var mime in accept)
            {
                if (mime.MediaType == "application/json")
                {
                    var value = mime.Parameters
                        .Where(x => x.Name.Equals("version", StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (value != null)
                        return value.Value;
                }
            }

            return "1";
        }

        private string GetVersionFromMediaType(HttpRequestMessage request)
        {
            var accept = request.Headers.Accept;
            var ex = new Regex(@"application\/vnd\.tutWebApi1\.([a-z]+)\.v([0-9]+)\+json", RegexOptions.IgnoreCase);

            foreach (var mime in accept)
            {
                var match = ex.Match(mime.MediaType);

                if (match != null)
                {
                    // get matched group two from regex, which is the version number
                    return match.Groups[2].Value;
                }
            }

            return "1";
        }

        private string GetVersionFromHeader(HttpRequestMessage request)
        {
            const string HEADER_NAME = "X-webApi1Tut-Version";

            if (request.Headers.Contains(HEADER_NAME))
            {
                var header = request.Headers.GetValues(HEADER_NAME).FirstOrDefault();

                if (header != null)
                    return header;
            }

            return "1";
        }

        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            var version = query["v"];

            if (version != null)
                return version;

            return "1";
        }
    }
}