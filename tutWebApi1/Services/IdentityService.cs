using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace tutWebApi1.Services
{
    public class IdentityService : IIdentityService
    {
        public string CurrentUser
        {
            get
            {
#if DEBUG
                return "yeasin";
#else
                return Thread.CurrentPrincipal.Identity.Name;
#endif

            }
        }
    }
}