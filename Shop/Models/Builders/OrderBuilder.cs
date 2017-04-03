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
        OrdersAdminPage BuildOrders(OrderFilter filter);
        List<OrderModel> OrdersModel(OrderFilter filter);
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
            model.Order.From=DateTime.Now.Date;
            model.Order.To=DateTime.Now.Date.AddDays(+1);
            model.Order.Products = SkuViewerBuilder.BuildListProductsByIds(ids);
            model.DeliveryTypes = dataService.List<DeliveryType>();
            model.PaymentTypes = dataService.List<PaymentType>();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

        public OrdersAdminPage BuildOrders(OrderFilter filter)
        {
            var model = new OrdersAdminPage();
            model.Orders = OrdersModel(filter);

            model.DeliveryTypes = dataService.List<DeliveryType>();
            model.PaymentTypes = dataService.List<PaymentType>();
            model.OrderStates = dataService.List<OrderState>();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

        public List<OrderModel> OrdersModel(OrderFilter filter)
        {
            var tmpOrders = dataService.ListOrdersByFilter(filter);
            return tmpOrders.Select(item => new OrderModel()
            {
                OrderId = item.Id,
                ClientFirstName = item.Client.name,
                ClientLastName = item.Client.lastName,
                ClientPhone = item.Client.mPhone,
                PaymentType = item.PaymentType,
                OrderState = item.OrderState,
                IsPaid = item.IsPaid,
                CreateDate = item.CreateDate,
                OrderNumber = item.OrderNumber,
                DeliveryType = item.DeliveryType
            }).ToList();
        } 

    }
}