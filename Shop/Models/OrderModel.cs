using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;

namespace Shop.Models
{
   public class OrderClientPage : MenuModel
    {
       public OrderModel Order { get; set; }
       public List<DeliveryType> DeliveryTypes { get; set; }
       public List<PaymentType> PaymentTypes { get; set; }

       public string Description { get; set; }
       public string Keywords { get; set; }

       public OrderClientPage()
       {
            Order=new OrderModel();
            DeliveryTypes=new List<DeliveryType>();
            PaymentTypes=new List<PaymentType>();
       }
    }

    public class OrderModel
    {
        public long OrderId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long ClientId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string City { get; set; }
        public string TypeStreet { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Level { get; set; }
        public string Flat { get; set; }
        public string PdfLink { get; set; }

        public int OrderNumber { get; set; }
        public int OrderPrefix { get; set; }
        public string OrderComment { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsPaid { get; set; }

        public bool IsHaveContract { get; set; }
        public DateTime PayDate { get; set; }
        public string PaymentId { get; set; }
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }

        public List<ShortSKUModel> Products  { get; set; }
        public List<OrderProduct> OrderProducts  { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderState OrderState { get; set; }

        public OrderModel()
        {
            OrderProducts = new List<OrderProduct>();
            Products=new List<ShortSKUModel>();
            DeliveryType=new DeliveryType();
            PaymentType=new PaymentType();
            OrderState = new OrderState();
        }
    }


    public class OrdersAdminPage : MenuModel
    {
        public List<OrderModel> Orders { get; set; }
        public List<OrderOneClick> OneClickOrders { get; set; }
        public List<DeliveryType> DeliveryTypes { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<OrderState> OrderStates { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        //public string Description { get; set; }
        //public string Keywords { get; set; }

        public OrdersAdminPage()
        {
            Orders = new List<OrderModel>();
            DeliveryTypes = new List<DeliveryType>();
            PaymentTypes = new List<PaymentType>();
            OrderStates = new List<OrderState>();
            From = DateTime.Now.Date.AddDays(-1).Date;
            To = DateTime.Now.Date.AddDays(1);
            OneClickOrders=new List<OrderOneClick>();
        }
    }
}