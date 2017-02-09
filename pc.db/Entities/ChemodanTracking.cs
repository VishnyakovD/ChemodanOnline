using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class ChemodanTracking
    {
		public virtual long Id { get; set; }
		public virtual long skuId { get; set; }
		public virtual string Code { get; set; }
		public virtual ChemodanLocation Location { get; set; }

		public ChemodanTracking() { }
    }
}
