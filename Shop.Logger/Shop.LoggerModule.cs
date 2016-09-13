using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Module = Autofac.Module;


namespace Shop.Logger
{

    public class LoggerModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register((c, p) => new Logger(p.TypedAs<Type>()))
                .AsImplementedInterfaces();
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing +=
                (sender, args) =>
                {
                    var forType = args.Component.Activator.LimitType;

                    var logParameter = new ResolvedParameter(
                        (p, c) => p.ParameterType == typeof(ILogger),
                        (p, c) => c.Resolve<ILogger>(TypedParameter.From(forType)));

                    args.Parameters = args.Parameters.Union(new[] { logParameter });
                };
        }
        //protected override void Load(ContainerBuilder builder)
        //{
        //    builder.RegisterType<Logger>().As<ILogger>();
        //}
    }
}
