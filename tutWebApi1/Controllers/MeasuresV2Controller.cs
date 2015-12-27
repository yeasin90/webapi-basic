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
    public class MeasuresV2Controller : BaseApiController
    {
        public MeasuresV2Controller(DataSource dataSource) : base(dataSource)
        {

        }

        public IEnumerable<MeasureV2Model> Get(int foodid)
        {
            return TheDataSource.GetMeasure(foodid).Select(x => TheModelFactory.Create2(x));
        }

        public MeasureModel Get(int foodid, int id)
        {
            return TheModelFactory.Create2(TheDataSource.GetMeasure(foodid).Where(x => x.Id == id).FirstOrDefault());
        }
    }
}
