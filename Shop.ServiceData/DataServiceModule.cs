using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Shop.DataService;
using Module = Autofac.Module;


namespace Shop.ServiceData
{
   
    public class DataServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataService.DataService>().As<IDataService>();
        }
    }
}
