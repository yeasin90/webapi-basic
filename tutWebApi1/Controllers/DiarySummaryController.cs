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
    public class DiarySummaryController : BaseApiController
    {
        private IIdentityService _identityService;

        public DiarySummaryController(DataSource dataSource, IIdentityService identityService) : base(dataSource)
        {
            _identityService = identityService;
        }

        public object Get(DateTime diaryId)
        {
            try
            {
                var diary = TheDataSource.GetDiary(_identityService.CurrentUser, diaryId);

                if (diary == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return TheModelFactory.CreateSummary(diary);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
