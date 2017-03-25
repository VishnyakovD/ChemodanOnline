using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.db.Entities
{
    public class OrderOneClick
    {
        public virtual long Id { get; set; }
        public virtual long ProductId { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string Phone { get; set; }
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool IsComplite { get; set; }

        public OrderOneClick()
        {

        }
    }

}