using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Sku:idName
    {
        public virtual decimal price { get; set; }
        public virtual decimal priceAct { get; set; }
        public virtual Brand brand { get; set; }
        public virtual string description { get; set; }
        public virtual Photo smalPhoto { get; set; }
        public virtual string articul { get; set; }
        public virtual bool isHide { get; set; }
        public virtual IList<PhotoBig> listPhoto { get; set; }
        public virtual IList<Category> listCategory { get; set; }
        public virtual IList<Specification> listSpecification { get; set; }
        public virtual IList<Comment> listComment { get; set; }
        public virtual string care { get; set; }
        public virtual ChemodanStatus chemodanStatus { get; set; }
        public virtual int chemodanDaysRent { get; set; }
        public virtual ChemodanProvider chemodanProvider { get; set; }
        public virtual ChemodanType chemodanType { get; set; }

        public Sku()
        {
            brand=new Brand();
            smalPhoto=new Photo();
            listPhoto=new List<PhotoBig>();
            listCategory=new List<Category>();
            listSpecification=new List<Specification>();
            listComment = new List<Comment>();
            chemodanStatus=new ChemodanStatus();
            chemodanProvider=new ChemodanProvider();
            chemodanType = new ChemodanType();
        }
    }
}
