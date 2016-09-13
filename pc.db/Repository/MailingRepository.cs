using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class MailingRepository : Repository<Mailing>
    {
        public MailingRepository(ISession session)
            : base(session)
        {
        }

        public IEnumerable<Mailing> AllMailingByActive(bool isActive)
        {
            return session.QueryOver<Mailing>()
                .Where(a => (a.isActive == isActive))
                .List();
        }

        public Mailing OneByEmail(string email)
        {
            var list = session.QueryOver<Mailing>().Where(a => (a.email==email)).List();
            if (list != null && list.Any())
            {
                return list.First();
            }
            return null;
        }



    }
}
