using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class idName
    {
        public virtual long id { get; set; }
        public virtual string name { get; set; }
        public virtual int sortPriority { get; set; }
        public virtual bool isUse { get; set; }
    }

    public enum DisplayType
    {
        MainCarusel=0,
        MainInfoBlock=1,
        Favorite=2,
        Default=999
    }
}
