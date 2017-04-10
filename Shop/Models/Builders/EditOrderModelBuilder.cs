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

            order.Order.OrderId = tmpOrder.Id;
            order.Order.ClientFirstName = tmpOrder.Client.name;
            order.Order.ClientLastName = tmpOrder.Client.lastName;
            order.Order.ClientPhone = tmpOrder.Client.mPhone;
            order.Order.OrderComment = tmpOrder.OrderComment;
            order.Order.ClientEmail = tmpOrder.Client.email;
            order.Order.PaymentType = tmpOrder.PaymentType;
            order.Order.OrderState = tmpOrder.OrderState;
            order.Order.IsPaid = tmpOrder.IsPaid;
            order.Order.CreateDate = tmpOrder.CreateDate;
            order.Order.OrderNumber = tmpOrder.OrderNumber;
            order.Order.DeliveryType = tmpOrder.DeliveryType;

            order.Order.City= tmpOrder.Client.editAdress.city;
            order.Order.TypeStreet = tmpOrder.Client.editAdress.typeStreet;
            order.Order.Street = tmpOrder.Client.editAdress.street;
            order.Order.Home = tmpOrder.Client.editAdress.numHome;
            order.Order.Level = tmpOrder.Client.editAdress.level;
            order.Order.Flat = tmpOrder.Client.editAdress.numFlat;

            order.Order.IsHaveContract = tmpOrder.IsHaveContract;
            order.Order.PayDate = tmpOrder.PayDate;
            order.Order.PaymentId = tmpOrder.PaymentId;
            order.Order.UserName = tmpOrder.UserName;
            order.Order.UserId = tmpOrder.UserId;
            order.Order.From = tmpOrder.From;
            order.Order.To = tmpOrder.To;

            order.Order.OrderProducts = tmpOrder.Products.ToList();

            order.topMenuItems = BuildTopMenu();
            return order;
        }

    }
}