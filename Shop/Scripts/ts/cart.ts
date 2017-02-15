

class CartItem {
    ProductId: number;
    Count: number;

    constructor(ProductId: number, Count: number) {
        this.ProductId = ProductId;
        this.Count = Count;
    }
}

class Cart {
    ListProducts: CartItem[];
    DateFrom: Date;
    DateTo: Date;

    constructor() {
        this.ListProducts = [];
        this.DateFrom = new Date();
        this.DateTo = new Date();
        this.GetCartCookie();
    }



    AddToCart(id: number, maxCount: number):void {
        var item = this.ListProducts.filter(item => item.ProductId === id);
        if (item.length > 0) {
            if (item[0].Count < maxCount) {
                item[0].Count += 1;
            } else {
                $("#ServerMessage").html("нельзя добавить");
            }
            
        } else {
            this.ListProducts.push(new CartItem(id, 1));
        }
        this.SetCartCookie();
        this.GetCartCookie();
    }

    RemoveFromCart(id: number): void {//не проверил
        var item = this.ListProducts.filter(item => item.ProductId === id);
        if (item.length > 0) {
            item[0].Count -= 1;

            if (item[0].Count<1) {
                this.ListProducts = this.ListProducts.filter(item => item.ProductId !== id);
            }
        } 

        this.SetCartCookie();
        this.GetCartCookie();
    }

    SetCartCookie(): void {
        var curDate = new Date();
        curDate.setMinutes(curDate.getMinutes() + 20);
        $.cookie("cart", JSON.stringify(this), { expires: curDate }); 
    }
    GetCartCookie(): void {

        if (!$.cookie("cart")) {
            return;
        }
        var cartCook = (<Cart>JSON.parse($.cookie("cart")));
        
        this.ListProducts = cartCook.ListProducts;
        this.DateFrom = cartCook.DateFrom;
        this.DateTo = cartCook.DateTo;
        console.log(this);
    }

}


var cart: Cart;

$(() => {

    if ($(".js-addcart").length>0) {
        cart = new Cart();

        $(document).on("click", ".js-addcart", (e) => {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            cart.AddToCart(itemId, itemMax);
        });
    }
});