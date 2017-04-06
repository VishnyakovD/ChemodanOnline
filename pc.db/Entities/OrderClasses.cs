using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.db.Entities
{
      public class Order
      {
        public virtual long Id { get; set; }

        public virtual int OrderNumber { get; set; }
        public virtual int OrderPrefix { get; set; }
        public virtual string OrderComment { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual bool IsPaid { get; set; }
        public virtual DateTime From { get; set; }
        public virtual DateTime To { get; set; }
        public virtual Client Client { get; set; }
        public virtual OrderInfoFile Pdf { get; set; }
        public virtual IList<OrderProduct> Products { get; set; }
        public virtual DeliveryType DeliveryType { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual OrderState OrderState { get; set; }


        public Order()
        {
          Client=new Client();
          Products =new List<OrderProduct>();
          DeliveryType = new DeliveryType();
          PaymentType = new PaymentType();
          OrderState=new OrderState();
        }
      }

      public class OrderProduct
      {
        public virtual long Id { get; set; }
        public virtual long ProductId { get; set; }
        public virtual long OrderId { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string Article { get; set; }
        public virtual string Code { get; set; }
        public virtual decimal PriceDay { get; set; }
        public virtual decimal FullPrice { get; set; }
        public virtual decimal NaturalPrice { get; set; }
        //public int Count { get; set; }

        public OrderProduct()
        {

        }
      }

      public class DeliveryType
      {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int SortProirity { get; set; }
        public virtual bool IsHide { get; set; }

        public DeliveryType()
        {

        }
      }

      public class PaymentType
      {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int SortProirity { get; set; }
        public virtual bool IsHide { get; set; }

        public PaymentType()
        {

        }
      }

      public class OrderInfoFile
      {
        public virtual long Id { get; set; }
        public virtual string FileLink { get; set; }

        public OrderInfoFile()
        {

        }
      }

      public class OrderState
      {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int SortProirity { get; set; }
        public virtual bool IsHide { get; set; }

        public OrderState()
        {

        }
      }


    public class OrderFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long DeliveryType { get; set; }
        public long OrderState { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsComplite { get; set; }


        public OrderFilter()
        {
            From = DateTime.Now.Date.AddDays(-1).Date;
            To = DateTime.Now.Date.AddDays(1);
        }
    }
}