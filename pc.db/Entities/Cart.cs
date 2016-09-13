using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Cart:idName
    {

        public virtual string nameClient { get; set; }
        public virtual string phone { get; set; }
        public virtual string email { get; set; }
        public virtual string city { get; set; }
        public virtual string street { get; set; }
        public virtual string numHome { get; set; }
        public virtual string numFlat { get; set; }
        public virtual string comment { get; set; }
        public virtual CartState state { get; set; }
        public virtual IList<CartItem> listSku { get; set; }
        public virtual int totalCount { get; set; }
        public virtual DateTime createDate { get; set; }


        public Cart()
        {
            listSku = new List<CartItem>();
            createDate=DateTime.Now;
     
        }

    }
}
