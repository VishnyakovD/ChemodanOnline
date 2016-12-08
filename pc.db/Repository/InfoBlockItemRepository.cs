using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class InfoBlockItemRepository : Repository<InfoBlockItem>
    {
        public InfoBlockItemRepository(ISession session)
            : base(session)
        {
        }


        public IEnumerable<InfoBlockItem> ListByType(DisplayType type)
        {
            return session.QueryOver<InfoBlockItem>()
                .Where(a => a.DisplayType== type)
                .List();
        }


    }
}
