using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tutWebApi1.Models;
using tutWepApi1.Repository;
using tutWepApi1.Repository.Entities;

namespace tutWebApi1.Controllers
{
    public class MeasuresController : BaseApiController
    {
        public MeasuresController(DataSource dataSource) : base(dataSource)
        {

        }

        public IEnumerable<MeasureModel> Get(int foodid)
        {
            return TheDataSource.GetMeasure(foodid).Select(x => TheModelFactory.Create(x));
        }

        public MeasureModel Get(int foodid, int id)
        {
            return TheModelFactory.Create(TheDataSource.GetMeasure(foodid).Where(x => x.Id == id).FirstOrDefault());
        }
    }
}
