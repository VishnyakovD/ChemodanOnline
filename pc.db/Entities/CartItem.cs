using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class CartItem:idName
    {
        public virtual long idSku { get; set; }
        public virtual int count { get; set; }
        public virtual decimal price { get; set; }
        public virtual decimal priceAct { get; set; }
        public virtual long CartId { get; set; }

        public CartItem()
        {
          
        }

    }
}
