using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Article
    {
        public virtual long id { get; set; }
        public virtual string title { get; set; }
        public virtual string body { get; set; }
        public virtual string imgPath { get; set; }
        public virtual string pathLink { get; set; }

        public Article() { }
    }
}
