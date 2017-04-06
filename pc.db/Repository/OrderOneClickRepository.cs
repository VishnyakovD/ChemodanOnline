using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class OrderOneClickRepository : Repository<OrderOneClick>
    {
        public OrderOneClickRepository(ISession session)
            : base(session)
        {
        }

        public List<OrderOneClick> AllOrdersOneClick(OrderFilter filter)
        {
            var query = session.QueryOver<OrderOneClick>().Where(order => order.CreateDate.Date >= filter.From && order.CreateDate <= filter.To);
            //if (filter.DeliveryType > 0)
            //{
            //    query = query.Where(order => order.DeliveryType.Id == filter.DeliveryType);
            //}
            //if (filter.OrderState > 0)
            //{
            //    query = query.Where(order => order.OrderState.Id == filter.OrderState);
            //}
            if (filter.IsComplite.HasValue)
            {
                query = query.Where(order => order.IsComplite == filter.IsComplite.Value);
            }
            return query.List().ToList();
        }
    }
}
