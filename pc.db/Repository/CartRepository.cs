using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class CartRepository : Repository<Cart>
    {
        public CartRepository(ISession session)
            : base(session)
        {
        }

        public IEnumerable<Cart> GetCartsByDateAndStatus(DateTime start, DateTime end, CartState state)
        {
            if (state != null)
            {
                return session.QueryOver<Cart>()
                 .Where(a => (a.createDate >= start.Date && a.createDate <= end.Date.AddDays(1).AddSeconds(-1) && a.state == state))
                .List(); 
            }
            return session.QueryOver<Cart>()
             .Where(a => (a.createDate >= start.Date && a.createDate <= end.Date.AddDays(1).AddSeconds(-1)))
            .List(); 
        }

    }
}
