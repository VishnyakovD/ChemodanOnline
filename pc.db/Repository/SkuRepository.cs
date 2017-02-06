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

        public IEnumerable<Sku> ListProductsByFilters(FilterFoDB filters)
        {

            IList<Sku> skus = null;
            IList<Category> cats=null;
            IList<Specification> spec=null;

            var idCats = filters.Categories.Select(fcat => fcat.Id).ToArray();
            var idTypes = filters.ChemodanTypes.Select(st => st.Id).ToArray();
            var idSpecs = filters.Specifications.Select(item=>item.Id).ToArray();
            var valueSpecs = filters.Specifications.Select(item=>item.Value).ToArray();

            if (idCats.Any() && skus == null)
            {
                cats = session.CreateCriteria(typeof (Category)).Add(Restrictions.In("staticcat", idCats)).List<Category>();
                skus = session.CreateCriteria(typeof(Sku))
                    .Add(Restrictions.In("id", cats.Select(c => c.skuId).ToArray()))
                    .Add(Restrictions.Eq("isHide", false))
                    .List<Sku>().ToList();

                idCats = null;
            }

            if (idTypes.Any() && skus == null)
            {
                skus = session.CreateCriteria(typeof(Sku))
                        .Add(Restrictions.In("chemodanType", idTypes))
                        .Add(Restrictions.Eq("isHide", false))
                        .List<Sku>();
                idTypes = null;
            }

            if (idSpecs.Any() && skus == null)
            {

                spec = session.QueryOver<Specification>()
                    .Where(s => s.staticspec.id.IsIn(idSpecs)
                    && s.value.IsIn(valueSpecs)).List();

                skus = session.QueryOver<Sku>()
                    .Where(sku => sku.id.IsIn(spec.Select(item => item.skuId).ToArray())&&sku.isHide==false).List();

                idSpecs = null;
            }



            if (idCats!=null && idCats.Any())
            {
                // skus = skus.Where(sku => sku.id.IsIn(cats.Select(c => c.skuId).ToArray())).ToList();
                skus = (from sku in skus from cat in sku.listCategory.Where(c => idCats.Contains(c.staticcat.id)) select sku).ToList();
            }

            if (idTypes != null && idTypes.Any())
            {
                //skus = skus.Where(sku => sku.chemodanType.id.IsIn(idTypes)).ToList();
                skus = skus.Where(sku => idTypes.Contains(sku.chemodanType.id)).ToList();
            }

            if (idSpecs != null&& idSpecs.Any())
            {

                var tmpSkus=new List<Sku>();
                foreach (var sku in skus)
                {
                    foreach (var skuSpec in sku.listSpecification)
                    {
                        foreach (var filterSpec in filters.Specifications)
                        {
                            if (filterSpec.Id== skuSpec.staticspec.id&&filterSpec.Value==skuSpec.value)
                            {
                                tmpSkus.Add(sku);
                            }
                        }
                    }
                }
                skus = tmpSkus;
                // skus = (from sku in skus from skuSpec in sku.listSpecification where filters.Specifications.Contains(new FilterItemValue {Id = skuSpec.staticspec.id, Value = skuSpec.value}) select sku).ToList();
            }
       






            return skus;
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
