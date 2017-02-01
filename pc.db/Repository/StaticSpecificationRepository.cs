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

        public FilterProduct ListFilters()
        {
            var result=new FilterProduct();

            var listSpec = session
                .Query<Specification>().GroupBy(gr => gr.staticspec).ToList();

            result.Specifications = listSpec.Select(gr => new FilterSpecification()
            {
                Specification = gr.Key,
                Values = gr.GroupBy(gr2=>gr2.value).
                    Select(gr2=>gr2.First()).
                    Select(it => new FilterItemValue()
                    {
                        Id = it.staticspec.id,
                        Value = it.value
                    }).ToArray()
            }).ToArray();

             return result;
        }

    }


   public enum FilterType
    {
        Specification=0,
        Category=1,
        ChemodanType=2
    }

    public class FilterProduct
    {
        public FilterCategory[] Categories { get; set; }
        public FilterChemodanTypes[] ChemodanTypes { get; set; }
        public FilterSpecification[] Specifications { get; set; }

        public FilterProduct()
        {
            Specifications = new FilterSpecification[] {}; 
            Categories =new FilterCategory[] {};
            ChemodanTypes=new FilterChemodanTypes[] {};
           
        }
    }

    public class FilterItemValue
    {
        public long Id { get; set; }
        public string Value { get; set; }
    }

  
    public class FilterSpecification
    {
        public StaticSpecification Specification { get; set; }
 
        public FilterItemValue[] Values { get; set; }

        public FilterSpecification()
        {
            Specification = new StaticSpecification();
            Values = new FilterItemValue[] { };
        }
    }

    public class FilterCategory
    {
        public List<FilterItemValue> Values { get; set; }

        public FilterCategory()
        {
            Values = new List<FilterItemValue>();
        }
    }

    public class FilterChemodanTypes
    {
        public List<FilterItemValue> Values { get; set; }

        public FilterChemodanTypes()
        {
            Values = new List<FilterItemValue>();
        }
    }





    public class FilterFoDB
    {
        public FilterItemValue[] Categories { get; set; }
        public FilterItemValue[] ChemodanTypes { get; set; }
        public FilterItemValue[] Specifications { get; set; }

        public FilterFoDB()
        {
            Specifications = new FilterItemValue[] { };
            Categories = new FilterItemValue[] { };
            ChemodanTypes = new FilterItemValue[] { };

        }
    }
}
