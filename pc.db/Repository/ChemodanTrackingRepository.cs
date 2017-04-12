using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class ChemodanTrackingRepository : Repository<ChemodanTracking>
    {
        public ChemodanTrackingRepository(ISession session)
            : base(session)
        {
        }

        public List<ChemodanTracking> AllByProductId(long productId)
        {
            return session.QueryOver<ChemodanTracking>()
                .Where(a =>a.skuId== productId)
                .List().ToList();
        }


    }
}
