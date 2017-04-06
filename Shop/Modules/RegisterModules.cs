using Autofac;
using Shop.Models.Builders;

namespace Shop.Modules
{
    public class RegisterModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdminModelBuilder>().As<IAdminModelBuilder>();

            builder.RegisterType<SKUModelBuilder>().As<ISKUModelBuilder>();

            builder.RegisterType<ArticleBuilder>().As<IArticleBuilder>();

            builder.RegisterType<SkuViewerBuilder>().As<ISkuViewerBuilder>();

            builder.RegisterType<AccountAdminModelBuilder>().As<IAccountAdminModelBuilder>();

            builder.RegisterType<RegisterBuilder>().As<IRegisterBuilder>();

            builder.RegisterType<ClientModelBuilder>().As<IClientModelBuilder>();

            builder.RegisterType<MainPageBuilder>().As<IMainPageBuilder>();

            builder.RegisterType<OrderBuilder>().As<IOrderBuilder>();

            builder.RegisterType<EditOrderModelBuilder>().As<IEditOrderModelBuilder>();

        }
    }
}