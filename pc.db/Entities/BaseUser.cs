using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class BaseUser
    {
        public virtual long id { get; set; }
        public virtual string name { get; set; }
        public virtual string lastName { get; set; }
        public virtual string patronymic { get; set; }
        public virtual string email { get; set; }
        public virtual string mPhone { get; set; }
        public virtual string passSeria { get; set; }
        public virtual string passNumber { get; set; }
        public virtual DateTime passDate { get; set; }
        public virtual string passIssuedBy { get; set; }
        //public List<GroupUser> GroupsUser { get; set; }
        public virtual bool isUse { get; set; }
        public virtual Adress adress { get; set; }
        public virtual EditAdress editAdress { get; set; }
        public virtual DateTime birthDate { get; set; }


        public BaseUser()
        {
            adress = new Adress();
            editAdress=new EditAdress();
            passDate=new DateTime(1930,01,01);
            birthDate = new DateTime(1930,01,01);
            // GroupsUser = new List<GroupUser>();
            // GroupsUser = new List<GroupUser>();
        }
    }
}
