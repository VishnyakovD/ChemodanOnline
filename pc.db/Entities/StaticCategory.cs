using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class StaticCategory:idName
    {
        public virtual decimal price { set; get; }
        public virtual string keywords { set; get; }
        public virtual string description { set; get; }
        public virtual string bodyText { set; get; }
    }
}
