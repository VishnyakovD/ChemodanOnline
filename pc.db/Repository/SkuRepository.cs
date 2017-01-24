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
    public class SkuRepository : Repository<Sku>
    {
        public SkuRepository(ISession session)
            : base(session)
        {
        }

        public IEnumerable<Sku> ListSkuByCategory(StaticCategory cat, bool isHide=false)
        {
            var ids = session.QueryOver<Category>().Where(i => i.staticcat == cat).List().Select(i => i.skuId).ToArray();
            var retval = session.QueryOver<Sku>().Where(s => s.id.IsIn(ids) && s.isHide == isHide).List();
            return retval;
        }

        public IEnumerable<Sku> AllByCategory(Category category, bool isHide = false)
        {
            return session.QueryOver<Sku>()
                .Where(a => (a.listCategory.Contains(category)) && a.isHide == isHide)
                .List();
        }

        public IEnumerable<Sku> AllByBrand(Brand brand, bool isHide = false)
        {
            return session
                .QueryOver<Sku>()
                .Where(a => a.brand == brand && a.isHide == isHide)
                .List();
        }

        public IEnumerable<Sku> AllHiddenSku(bool isHide = false)
        {
            return session.QueryOver<Sku>()
                .Where(a => a.isHide == isHide).List();
        }

        public IEnumerable<Sku> AllByDisplayType(DisplayType type)
        {
            return session.QueryOver<Sku>()
                .Where(a => a.displayType == type).List();
        }

    }
}
