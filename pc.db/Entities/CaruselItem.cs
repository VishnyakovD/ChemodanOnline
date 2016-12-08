using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.db.Entities
{
    public class CaruselItem : idName
    {
        public virtual string Title { set; get; }
        public virtual string Link { set; get; }
        public virtual string Description { set; get; }
        public virtual string Image { set; get; }
        public virtual DisplayType DisplayType { set; get; }

        public CaruselItem()
        {
        }
    }

}