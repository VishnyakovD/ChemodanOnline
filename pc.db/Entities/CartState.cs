using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class CartState:idName
    {
        public virtual int value { get; set; }
        public CartState() { }
    }
}

//Pending     Ожидает       1
//Confirmed   Подтвержден   2
//Cancelled   Отменен       3
//Refunded    Возвращен     4
//Shipped     Отгружен      5
//Paid        Оплачен       6
//Complete    Завершен      7



