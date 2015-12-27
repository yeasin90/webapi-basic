using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tutWepApi1.Repository
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DataSource>().ToSelf();
        }
    }
}
