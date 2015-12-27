using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using tutWebApi1.Filters;
using tutWebApi1.Models;
using tutWebApi1.Services;
using tutWepApi1.Repository;

namespace tutWebApi1.Controllers
{
    // authorization of web api 1
    [TestBasicAuthorize]
    public class DiariesController : BaseApiController
    {
        private IIdentityService _identityService;

        //private IIdentityService

        public DiariesController(DataSource dataSource, IIdentityService identityService) : base(dataSource)
        {
            _identityService = identityService;
        }

        // here, return type of the function is IEnumberable<DiaryModel>
        // but, when we make api call, we will get either xml or json (depending on how Accept header has been set)
        // Even, if we used object as return type, api will still would return the type based on Accept
        // then why thi IEnumberable<string> here?
        // This is just for fine compile time checking.
        // And Accpet header will be interpreted by a .NET class called Formatter (see WepApi.Config)
        // Check : FoodsController to see when to use object as return type in Api action result :)

        public IEnumerable<DiaryModel> Get()
        {
            var username = _identityService.CurrentUser;
            var result = TheDataSource.GetDiaries(username).Select(x => TheModelFactory.Create(x));

            return result;
        }

        // here instead of Returning Model, we are returning HttpReponseMessgae
        // this is because, in case if our uery returns null, then we should retunr some kind of Http Status code, as this is an API
        // this is the reason
        public HttpResponseMessage Get(DateTime diaryId)
        {
            var username = _identityService.CurrentUser;
            var result = TheDataSource.GetDiary(username, diaryId);

            if(result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(result));
        }
    }
}
