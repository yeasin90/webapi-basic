using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tutWebApi1.Models;
using tutWebApi1.Services;
using tutWepApi1.Repository;

namespace tutWebApi1.Controllers
{
    public class DiaryEntriesController : BaseApiController
    {
        private IIdentityService _identityService;

        public DiaryEntriesController(DataSource dataSource, IIdentityService identityService) : base(dataSource)
        {
            _identityService = identityService;
        }

        public IEnumerable<DiaryEntryModel> Get(DateTime diaryId)
        {
            return TheDataSource.GetDiaryEntries(diaryId, _identityService.CurrentUser).Select(x => TheModelFactory.Create(x));
        }

        public HttpResponseMessage Get(DateTime diaryId, int id)
        {
            var result = TheDataSource.GetDiarEntry(diaryId, _identityService.CurrentUser, id);

            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(result));
        }

        // POST method HTTP request : 
        // Content-Type: application/json

        // Request body for post : 
        // {
        //      "input1": 10,
        //      "input2": "Hello World"
        // }
        public HttpResponseMessage Post(DateTime diaryId, [FromBody]DiaryEntryModel model)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE method of HTTP :
        // DELETE method of HTTP cannot have a body
        // Fiddler http url : http://localhost:3750/api/user/diaries/2015-12-1/entries/1
        public HttpResponseMessage Delete(DateTime diaryId, int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // We can alos use PUT http method
        // But PUT expect the whole model in the request body
        // But, we may want partial update, example : only a specific propertie of the model to be updated
        // In that case, Patch is the appropiate
        // Fiddler request body : 
        // {
        //      "input1": 20
        // }
        // and method will be PATCH
        // But we may also need to update the whole model. In that case, we need PUT also
        // There is a way to use a method for both PUT and PATCH
        // This is done via Method Attribute
        [HttpPut]
        [HttpPatch]
        public HttpResponseMessage Patch(DateTime diaryId, int id, [FromBody] DiaryEntryModel model)
        {
            try
            {
                var entity = TheDataSource.GetDiarEntry(diaryId, _identityService.CurrentUser, id);

                if (entity == null) Request.CreateResponse(HttpStatusCode.NotFound);

                

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
