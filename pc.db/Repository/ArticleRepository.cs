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
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(ISession session)
            : base(session)
        {
        }

    }
}
