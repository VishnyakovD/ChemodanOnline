using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class CaruselItemRepository : Repository<CaruselItem>
    {
        public CaruselItemRepository(ISession session)
            : base(session)
        {
        }

        public IEnumerable<CaruselItem> ListByType(DisplayType type)
        {
            return session.QueryOver<CaruselItem>()
                .Where(a => a.DisplayType == type)
                .List();
        }


    }
}
