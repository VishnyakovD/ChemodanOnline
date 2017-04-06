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
        public EditOrderModelBuilder(IDataService dataService, IImagesPath imagesPath)
            : base(dataService, imagesPath)
        {

        }

        public EditOrderModel Build(long id)
        {
            var model = new EditOrderModel();
            var tmpOrder = dataService.Get<Order>(id);

            model.DeliveryTypes = dataService.List<DeliveryType>();
            model.PaymentTypes = dataService.List<PaymentType>();
            model.OrderStates = dataService.List<OrderState>();

            model.Order.OrderId = tmpOrder.Id;
            model.Order.ClientFirstName = tmpOrder.Client.name;
            model.Order.ClientLastName = tmpOrder.Client.lastName;
            model.Order.ClientPhone = tmpOrder.Client.mPhone;
            model.Order.PaymentType = tmpOrder.PaymentType;
            model.Order.OrderState = tmpOrder.OrderState;
            model.Order.IsPaid = tmpOrder.IsPaid;
            model.Order.CreateDate = tmpOrder.CreateDate;
            model.Order.OrderNumber = tmpOrder.OrderNumber;
            model.Order.DeliveryType = tmpOrder.DeliveryType;
           // model.Order.Products = tmpOrder.Products.Select(item=>new ShortSKUModel() {})


            model.topMenuItems = BuildTopMenu();
            return model;
        }

    }
}