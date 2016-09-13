using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Specification
    {
        public virtual long id { get; set; }
        public virtual long skuId { get; set; }
        public virtual StaticSpecification staticspec { get; set; }
        public virtual string value { get; set; }
        public virtual string type { get; set; }
    }
}
