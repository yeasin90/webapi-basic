using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tutWebApi1.ActionResults;
using tutWebApi1.Models;
using tutWepApi1.Repository;

namespace tutWebApi1.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        DataSource _dataSource;
        ModelFactory _modelFactory;

        public BaseApiController(DataSource dataSource)
        {
            _dataSource = dataSource;
        }

        protected DataSource TheDataSource
        {
            get
            {
                return _dataSource;
            }
        }

        // This pattern is called deffered initialization
        // We need to pass this.Request to a Model from a Controller
        // this.Request will get initializted after the contructor of a controller has been invoked.
        // So, deffered initialization
        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                    _modelFactory = new ModelFactory(this.Request);

                return _modelFactory;
            }
        }

        protected IHttpActionResult Versioned<T>(T body, string version="v1") where T : class
        {
            return new VersionedActionResult<T>(Request, version, body);
        }
    }
}
