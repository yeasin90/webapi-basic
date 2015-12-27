using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using tutWebApi1.Filters;
using tutWebApi1.Models;
using tutWepApi1.Repository;
using tutWepApi1.Repository.Entities;

namespace tutWebApi1.Controllers
{
    // here we are telling that, we don;t need user based authentication
    // but we do need authorization for api user
    // or, developer of this API must have an api key and secrte token
    // or, in order to use this controller from api as a developer, when a user hits : http://localhost:8901/api/nutrition/food, they must provide there api key and token
    // example : http://localhost:8901/api/nutrition/food?apiKey=XXXX&token=XXXX
    [TestBasicAuthorize(false)]
    [RoutePrefix("api/nutrition/foods")]
    public class FoodsController : BaseApiController
    {
        const int PAGE_SIZE = 2;

        public FoodsController(DataSource dataSource) : base(dataSource)
        {

        }

        // for includeMeasures = false, api call would be api/nutrition/food?includeMeasures=falsse
        [Route("", Name = "Foods")]
        public object Get(bool includeMeasures = true, int page = 0)
        {
            IEnumerable<Food> query;

            if (includeMeasures)
            {
                query = TheDataSource.GetFoodsWithMeasures();
            }
            else
            {
                query = TheDataSource.GetAllFoods();
            }

            var baseQuery = query.OrderBy(x => x.Description);

            var totalCount = baseQuery.Count();
            var totalPages = Math.Ceiling((double)totalCount / PAGE_SIZE);

            var helper = new UrlHelper(Request);
            var prevUrl = page > 0 ? helper.Link("Foods", new { page = page - 1 }) : "";
            var nextUrl = page < totalPages - 1 ? helper.Link("Foods", new { page = page + 1 }) : "";

            var results = query.Skip(PAGE_SIZE * page)
                               .Take(PAGE_SIZE)
                               .Select(x => TheModelFactory.Create(x));

            // retunr type of action is object
            // so the below will be serialized into JSON
            return new
            {
                TotalCount = totalCount,
                TotalPage = totalPages,
                PrevPageUrl = prevUrl,
                NextPageUrl = nextUrl,
                Results = results
            };
        }

        [Route("{foodid}", Name = "Food")] // Just like WebApi.Config route name
        public IHttpActionResult Get(int foodid)
        {
            return Versioned(TheDataSource.GetFoodsWithMeasures().Where(x => x.Id == foodid).Select(x => TheModelFactory.Create(x)).FirstOrDefault());
            //return TheModelFactory.Create(TheDataSource.GetFoodsWithMeasures.Where(x => x.Id == foodid).FirstOrDefault());
        }

    }
}
