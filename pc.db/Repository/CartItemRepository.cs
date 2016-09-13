using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class CartItemRepository : Repository<CartItem>
    {
        public CartItemRepository(ISession session)
            : base(session)
        {
        }

        public IEnumerable<CartItem> AllByCartId(long idCart)
        {
            return session.QueryOver<CartItem>()
                .Where(a => (a.CartId==idCart))
                .List();
        }

        public CartItem OneByIdSkuAndIdCart(long idCart, long idsku)
        {
            var list = session.QueryOver<CartItem>().Where(a => (a.CartId == idCart && a.idSku == idsku)).List();
            if (list != null && list.Any())
            {
                return list.First();
            }
            return null;
        }

        public bool RemoveSkuCartId(long idCart, long idsku)
        {
            var sku = OneByIdSkuAndIdCart(idCart, idsku);
            if (sku!=null)
            {
                Remove(sku);
                return true;
            }
            return false;
        }
    }
}
