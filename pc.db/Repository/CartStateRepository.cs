using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class CartStateRepository : Repository<CartState>
    {
        public CartStateRepository(ISession session)
            : base(session)
        {
        }



    }
}
