using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.db.Entities;

namespace Shop.Models
{
    public class ClientModel : MenuModel
    {
        public List<Client> list { set; get; }

        public ClientModel()
        {
            list = new List<Client>();
        }
    }

    public class ClientDataModel : MenuModel
    {
        public Client client { set; get; }
        public List<Sex> sexs { set; get; }

        public ClientDataModel()
        {
            sexs=new List<Sex>();
            client = new Client();
        }
    }
}