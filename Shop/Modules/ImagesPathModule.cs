using System.Web.Configuration;
using Autofac;
using Autofac.Core;
using Shop.Modules;


namespace Shop
{
    public class ImagesPathModule : Module
    {
        private readonly string _imagesPath;

        public ImagesPathModule()
        {
            this._imagesPath = WebConfigurationManager.AppSettings["ImagesPath"];
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ImagesPath>().WithParameter(ResolvedParameter.ForNamed<string>("imagePath")).As<IImagesPath>();
            builder.Register(ctx => _imagesPath).Named<string>("imagePath");

           

        }
    }
}