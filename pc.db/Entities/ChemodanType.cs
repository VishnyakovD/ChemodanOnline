using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class ChemodanType:idName
    {
        public virtual decimal priceDay { get; set; }

        public ChemodanType()
        {

        }
    }
}
