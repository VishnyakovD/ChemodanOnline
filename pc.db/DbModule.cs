using Autofac;
using Autofac.Core;

namespace Shop.db
{
    public class DbModule : Module
    {
        private readonly string _connectionString;

        public DbModule(string connectionString)
        {
            this._connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<DB>().WithParameter(ResolvedParameter.ForNamed<string>("connstring")).As<IDb>();

            builder.Register(ctx => _connectionString).Named<string>("connstring");
        }
    }
}