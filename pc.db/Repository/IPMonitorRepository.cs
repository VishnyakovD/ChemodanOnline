using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class IPMonitorRepository : Repository<IPMonitor>
    {
        public IPMonitorRepository(ISession session)
            : base(session)
        {
        }



    }
}
