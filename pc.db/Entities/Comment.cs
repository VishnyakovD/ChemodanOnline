using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Comment : idName
    {
        public virtual long skuId { get; set; }
        public virtual long userId { get; set; }
        public virtual long commentId { get; set; }
        public virtual DateTime createDate { get; set; }
        public virtual string text { get; set; }
    }
}
