using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace tutWebApi1.Services
{
    public class NinjectWebApiFilterProvider : IFilterProvider
    {
        private IKernel _kernel;

        public NinjectWebApiFilterProvider(IKernel kernel)
        {
            _kernel = kernel;
        }

        // this method : Get the filters for our system.
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            // get all the controller level filter
            var controllerFilters = actionDescriptor.ControllerDescriptor.GetFilters().Select(instance => new FilterInfo(instance, FilterScope.Controller));
            // get all the Action level filter
            var actionFilter = actionDescriptor.ControllerDescriptor.GetFilters().Select(instance => new FilterInfo(instance, FilterScope.Action));

            // make a list of all filters of the system (Controller level + Action Level)
            var filters = controllerFilters.Concat(actionFilter);

            foreach(var f in filters)
            {
                // do injection
                _kernel.Inject(f.Instance);
            }

            return filters;
        }
    }
}