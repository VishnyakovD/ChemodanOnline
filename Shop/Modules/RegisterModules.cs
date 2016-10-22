using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using Autofac;
using Shop.Models;
using Shop.Models.Builders;

namespace Shop
{
    public class RegisterModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdminModelBuilder>().As<IAdminModelBuilder>();

            builder.RegisterType<SKUModelBuilder>().As<ISKUModelBuilder>();

            builder.RegisterType<ArticleBuilder>().As<IArticleBuilder>();

            builder.RegisterType<SkuViewerBuilder>().As<ISkuViewerBuilder>();

            builder.RegisterType<CartBuilder>().As<ICartBuilder>();

            builder.RegisterType<AccountAdminModelBuilder>().As<IAccountAdminModelBuilder>();

            builder.RegisterType<RegisterBuilder>().As<IRegisterBuilder>();

            builder.RegisterType<ClientModelBuilder>().As<IClientModelBuilder>();

        }
    }
}