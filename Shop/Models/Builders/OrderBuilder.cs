using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Modules;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{

    public interface IOrderBuilder
    {
        OrderClientPage Build(long[] ids);
    }

    public class OrderBuilder : MenuBuilder, IOrderBuilder
    {
        private IImagesPath ImagesPath;
        private ISkuViewerBuilder SkuViewerBuilder;
        public long DefaultValueHasInStock { set; get; }
        public OrderBuilder(IDataService dataService, IImagesPath imagesPath, ISkuViewerBuilder skuViewerBuilder)
            : base(dataService, imagesPath)
        {
            this.ImagesPath = imagesPath;
            this.SkuViewerBuilder = skuViewerBuilder;
            DefaultValueHasInStock = long.Parse(WebConfigurationManager.AppSettings["DefaultValueHasInStock"]);
        }

        public OrderClientPage Build(long[] ids)
        {
            var model = new OrderClientPage();
            model.Order.From=DateTime.Now;
            model.Order.To=DateTime.Now.AddDays(+1);
            model.Order.Products = SkuViewerBuilder.BuildListProductsByIds(ids);
            model.DeliveryTypes = dataService.List<DeliveryType>();
            model.PaymentTypes = dataService.List<PaymentType>();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

    }
}