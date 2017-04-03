using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(ISession session)
            : base(session)
        {
        }

        public List<Order> AllByFilters(OrderFilter filter)
        {
            var query= session.QueryOver<Order>().Where(order => order.CreateDate.Date >= filter.From && order.CreateDate <= filter.To);
            if (filter.DeliveryType>0)
            {
                query = query.Where(order => order.DeliveryType.Id == filter.DeliveryType);
            }
            if (filter.OrderState > 0)
            {
                query = query.Where(order => order.OrderState.Id == filter.OrderState);
            }
            if (filter.IsPaid.HasValue)
            {
                query = query.Where(order => order.IsPaid == filter.IsPaid.Value);
            }
            return query.List().ToList();
        }

        //public List<Order> AllByDates(DateTime from, DateTime to)
        //{
        //    return session.QueryOver<Order>()
        //        .Where(order => order.From.Date>=from&&order.To.Date.AddDays(1).AddSeconds(-1)<=to)
        //        .List().ToList();
        //}

        public List<Order> AllByClientPhone(string phone)
        {
            return session.QueryOver<Order>()
                .Where(order => order.Client.mPhone==phone)
                .List() as List<Order>;
        }

        public List<Order> AllByDeliveryType(long deliveryType)
        {
            return session.QueryOver<Order>()
                .Where(order => order.DeliveryType.Id== deliveryType)
                .List() as List<Order>;
        }

        public List<Order> AllByPaymentType(long paymentType)
        {
            return session.QueryOver<Order>()
                .Where(order => order.PaymentType.Id == paymentType)
                .List() as List<Order>;
        }

        public List<Order> AllByOrderState(long state)
        {
            return session.QueryOver<Order>()
                .Where(order => order.OrderState.Id == state)
                .List() as List<Order>;
        }

        public Order OneByOrderNumber(int num)
        {
            return session.QueryOver<Order>()
                .Where(order => order.OrderNumber == num)
                .List().First();
        }


    }
}
