using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class StaticSpecificationRepository : Repository<StaticSpecification>
    {
        public StaticSpecificationRepository(ISession session)
            : base(session)
        {
        }

        public IEnumerable<FilterSpecification> ListFilters()
        {
            var result=new List<FilterSpecification>();

            var listSpec = session
                .Query<Specification>().GroupBy(gr => gr.staticspec).ToList();

            result = listSpec.Select(gr => new FilterSpecification()
            {
                Specification = gr.Key,
                Values = gr.GroupBy(gr2=>gr2.value).
                Select(gr2=>gr2.First()).
                Select(it => new FilterValueItem()
                {
                    IsSelected = false,
                    Value = it.value
                }).ToList()
            }).ToList();

             return result;
        }

    }

    public class FilterSpecification
    {
        public StaticSpecification Specification { get; set; }
        public List<FilterValueItem> Values { get; set; }

        public FilterSpecification()
        {
            Specification=new StaticSpecification();
            Values=new List<FilterValueItem>();
        }
    }

    public class FilterValueItem
    {
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public FilterValueItem()
        {
            
        }
    }
}
