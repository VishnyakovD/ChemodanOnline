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

    public interface IEditOrderModelBuilder
    {
        EditOrderModel Build(long id);
        OrderModel BuildOrderModel(long id);

    }

    public class EditOrderModelBuilder : MenuBuilder, IEditOrderModelBuilder
    {
        ISkuViewerBuilder SkuViewerBuilder;
        public EditOrderModelBuilder(IDataService dataService, IImagesPath imagesPath,ISkuViewerBuilder skuViewerBuilder)
            : base(dataService, imagesPath)
        {
            SkuViewerBuilder = skuViewerBuilder;
        }

        public EditOrderModel Build(long id)
        {
            var order = new EditOrderModel();
            var tmpOrder = dataService.Get<Order>(id);

            order.DeliveryTypes = dataService.List<DeliveryType>();
            order.PaymentTypes = dataService.List<PaymentType>();
            order.OrderStates = dataService.List<OrderState>();

            order.Order = BuildOrderModel(id);

            order.topMenuItems = BuildTopMenu();
            return order;
        }

        public OrderModel BuildOrderModel(long id)
        {
            var order = dataService.Get<Order>(id);
            var orderModel = new OrderModel();

            orderModel.OrderId = order.Id;
            orderModel.ClientFirstName = order.Client.name;
            orderModel.ClientLastName = order.Client.lastName;
            orderModel.ClientPhone = order.Client.mPhone;
            orderModel.OrderComment = order.OrderComment;
            orderModel.ClientEmail = order.Client.email;
            orderModel.PaymentType = order.PaymentType;
            orderModel.OrderState = order.OrderState;
            orderModel.IsPaid = order.IsPaid;
            orderModel.CreateDate = order.CreateDate;
            orderModel.OrderNumber = order.OrderNumber;
            orderModel.DeliveryType = order.DeliveryType;
            if (order.Client.editAdress!=null)
            {
                orderModel.City = order.Client.editAdress.city;
                orderModel.TypeStreet = order.Client.editAdress.typeStreet;
                orderModel.Street = order.Client.editAdress.street;
                orderModel.Home = order.Client.editAdress.numHome;
                orderModel.Level = order.Client.editAdress.level;
                orderModel.Flat = order.Client.editAdress.numFlat;
            }


            orderModel.IsHaveContract = order.IsHaveContract;
            orderModel.PayDate = order.PayDate;
            orderModel.PaymentId = order.PaymentId;
            orderModel.UserName = order.UserName;
            orderModel.UserId = order.UserId;
            orderModel.From = order.From;
            orderModel.To = order.To;

            orderModel.OrderProducts = order.Products.ToList();

            return orderModel;
        }

    }
}