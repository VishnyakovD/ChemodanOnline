using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;

namespace Shop.Models
{
    public class MailingModel : MenuModel
    {
        public List<Mailing> mailings { set; get; }

        public MailingModel()
        {
            mailings=new List<Mailing>();
        }
    }
}