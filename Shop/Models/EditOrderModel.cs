using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;

namespace Shop.Models
{
    public class EditOrderModel : MenuModel
    {
        public OrderModel Order { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderState OrderState { get; set; }

        public List<DeliveryType> DeliveryTypes { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<OrderState> OrderStates { get; set; }

        public EditOrderModel()
        {
            Order=new OrderModel();
            DeliveryType = new DeliveryType();
            PaymentType = new PaymentType();
            OrderState = new OrderState();

            DeliveryTypes = new List<DeliveryType>();
            PaymentTypes = new List<PaymentType>();
            OrderStates = new List<OrderState>();
        }
    }
}