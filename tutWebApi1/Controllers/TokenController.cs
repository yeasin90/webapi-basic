using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using tutWebApi1.Areas.HelpPage.Models;
using tutWepApi1.Repository;
using tutWepApi1.Repository.Entities;

namespace tutWebApi1.Controllers
{
    public class TokenController : BaseApiController
    {
        public TokenController(DataSource dataSource) : base(dataSource)
        {

        }

        // fiddlr : 
        // url : http://localhost:8901/api/token
        // method : POST
        // request body : 
        // {
        //      "apiKey" : "",
        //      "signature: ""
        // }
        public HttpResponseMessage Post([FromBody] TokenRequestModel model)
        {
            try
            {
                // here user means developer who has assigend an Api public key
                var user = TheDataSource.GetUsers().Where(x => x.AppId == model.ApiKey).FirstOrDefault();

                if (user != null)
                {
                    // user (or developer) will send his Secret in a Base64 encoded format
                    var secret = user.Secret;

                    // we will not store raw Secret key for each developer
                    // that's why the below code
                    var key = Convert.FromBase64String(secret);
                    var provider = new System.Security.Cryptography.HMACSHA256(key);
                    var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(user.AppId));
                    var signature = Convert.ToBase64String(hash);

                    if(signature == model.Signature)
                    {
                        // create the access token
                        var rawTokenInfo = string.Concat(user.AppId + DateTime.UtcNow.ToString("d"));
                        var rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
                        var token = provider.ComputeHash(rawTokenByte);
                        var authToken = new AuthToken()
                        {
                            Token = Convert.ToBase64String(token),
                            Expiration = DateTime.UtcNow.AddDays(7),
                            ApiUser = user
                        };

                        // now perform insert for this token
                        // repo.Insert(authToken)
                        return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(authToken));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "error");
        }
    }
}
