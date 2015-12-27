using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using tutWepApi1.Repository;

namespace tutWebApi1.Controllers
{
    // begining part of the controller level for this route in below
    [RoutePrefix("api/stats")]
    //[EnableCors("*","*","*")] -> Got to WebConfig to Enable cors for the entire controllers
    public class StatsController : BaseApiController
    {
        public StatsController(DataSource dataSource) : base(dataSource)
        {

        }

        [Route("")]
        [DisableCors()] // Disbale CORS for specific method
        public IHttpActionResult Get()
        {
            var results = new
            {
                NumFoods = TheDataSource.GetAllFoods().Count,
                NumUsers = TheDataSource.GetUsers().Count
            };

            return Ok(results);
        }

        
        [Route("{id:int}")] // :int constrain
        //[EnableCors("*", "*", "GET")]
        public IHttpActionResult Get(int id)
        {
            if (id == 1)
                return Ok(new
                {
                    NumFoods = TheDataSource.GetAllFoods().Count
                });

            if(id == 2)
                return Ok(new
                {
                    NumUsers = TheDataSource.GetUsers().Count
                });

            return NotFound();
        }

        // ~ means overriding, or to use the RoutePrefix for all the routes except the below
        [Route("~/api/stat/{name:alpha}")] // :alpha is contrain
        public IHttpActionResult Get(string name)
        {
            if (name == "foods")
                return Ok(new
                {
                    NumFoods = TheDataSource.GetAllFoods().Count
                });

            if (name == "users")
                return Ok(new
                {
                    NumUsers = TheDataSource.GetUsers().Count
                });

            return BadRequest();
        }
    }
}
