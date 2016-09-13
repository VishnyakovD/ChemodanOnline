using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Shop.db.Entities;

namespace Shop.Models
{
    public class CartModel:MenuModel
    {
        public Cart cart { set; get; }
        public List<Sku> listSku { set; get; }
        public List<CartState> listState { set; get; }

        public CartModel()
        {
            listSku=new List<Sku>();
            cart=new Cart();
            listState = new List<CartState>();
        }
    }

    public class ListCartsModel : MenuModel
    {
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }
        public CartState state { set; get; }
        public List<CartState> listState { set; get; }

        public List<CartModel> listCarts { set; get; }

        public ListCartsModel()
        {
            listCarts = new List<CartModel>();
            startDate = DateTime.Now;
            endDate = DateTime.Now;
            state = new CartState();
            listState = new List<CartState>();

        }
    }
}