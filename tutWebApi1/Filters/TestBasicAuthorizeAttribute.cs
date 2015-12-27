#if DEBUG
#define DISABLE_SECURITY
#endif
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using tutWepApi1.Repository;

namespace tutWebApi1.Filters
{
    public class TestBasicAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private bool _perUser;

        // because this is a Filter and we don;t have enough controler over filter's ctor, we cannot perfomr ctor injection via Ninject
        // below by secifying [Inject] over property, we are uisng property injection
        // but to make the below code work for Filter, we need to to some tasks.
        // 1. Add a class that will be injected in the pipeline of API when the filters are initialized
        // 2. Name the class NinjectWebApiFilterProvider in Services
        // 3. Go to NinjectWebCommon and register NinjectWebApiFilterProvider with WebApi 
        // or do the below : 
        // GlobalConfiguration.Configuration.Services.Add(typeof(IFilterProvider), new NinjectWebApiFilterProvider(kernel));
        [Inject]
        private DataSource _dataSource { get; set; }

        // In an API, there may be two types of user : 
        // 1. token user, or user who is using our api for development
        // 2. Basic user, or user who is using our api as a service like : to view his diaries..etc

        public TestBasicAuthorizeAttribute(bool perUser = true)
        {
            _perUser = perUser;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
#if !DISABLE_SECURITY

            const string APIKEYNAME = "apikey";
            const string TOKENNAME = "token";

            var query = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);

            if (!string.IsNullOrWhiteSpace(query[APIKEYNAME]) &&
                !string.IsNullOrWhiteSpace(query[TOKENNAME]))
            {
                var apiKey = query[APIKEYNAME];
                var token = query[TOKENNAME];

                var authToken = _dataSource.GetAuthToken(token);

                if (authToken != null && authToken.ApiUser.AppId == apiKey && authToken.Expiration > DateTime.UtcNow)
                {

                    if (_perUser)
                    {
                        if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        {
                            return;
                        }

                        var authHeader = actionContext.Request.Headers.Authorization;

                        // Custom Basic authentication
                        if (authHeader != null)
                        {
                            // Check if the request contains type of authentication in the header and
                            // and authentication credentials in the request header
                            // request header in Fiddler (for this specific custom simple basic authentication): 
                            // url : http://localhost:8901/api/nutrition/food?apiKey=XXXX&token=XXXX
                            // Authorization: Basic yourBase64encodedusername:yourbase64encodedpassword (username and password should be base64 encoded in iso-8859-1 encoding, go to : base64encode.org)
                            if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase)
                                && !string.IsNullOrWhiteSpace(authHeader.Parameter))
                            {
                                // everything is valid
                                var rawCredentials = authHeader.Parameter;
                                var encoding = Encoding.GetEncoding("iso-8859-1");
                                var credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));
                                var split = credentials.Split(':');
                                var username = split[0];
                                var password = split[1];

                                // if valid user thn
                                var principal = new GenericPrincipal(new GenericIdentity(username), null);
                                // as we are using : Thread.CurrentPrincipal.Identity.Name to getitng the current user, below line is must
                                Thread.CurrentPrincipal = principal;
                                return;
                            }

                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            HandleUnauthorizes(actionContext);
#endif
        }

        private void HandleUnauthorizes(HttpActionContext actionContext)
        {
            // below will be used only for token aunthetication, not for peruser authentication
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            // telling how authentication will work
            // this is a self made header
            // below will work for basic authenctication or per user authenctication
            if (_perUser)
                actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='tutWebApi1' location='http://localhost:8901/account/login'");
        }
    }
}