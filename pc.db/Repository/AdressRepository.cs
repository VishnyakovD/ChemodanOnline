﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class AdressRepository : Repository<Adress>
    {
        public AdressRepository(ISession session)
            : base(session)
        {
        }



    }
}
