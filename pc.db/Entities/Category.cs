using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Category
    {
        public virtual long id { get; set; }
        public virtual long skuId { get; set; }
        public virtual StaticCategory staticcat { get; set; }
        public virtual string description { get; set; }
        public virtual string photoPath { get; set; }

        public Category()
        {
            
        }
    }
}
