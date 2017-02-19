using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class MenuItemRepository : Repository<MenuItem>
    {
        public MenuItemRepository(ISession session)
            : base(session)
        {
        }

        public List<MenuItem> AllByType(int type)
        {
            var menuitem=session.QueryOver<MenuItem>()
                .Where(a => (a.type==type))
                .List()
                .OrderBy(it => it.sortPriority)
                .ToList();
            return menuitem;
        }



    }
}
