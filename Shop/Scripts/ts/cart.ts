

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



    AddToCart(id:number):void {
        var item = this.ListProducts.filter(item => item.ProductId === id);
        if (item.length>0) {
            item[0].Count += 1;
        } else {
            this.ListProducts.push(new CartItem(id, 1));
        }
        this.SetCartCookie();
        this.GetCartCookie();
    }

    SetCartCookie():void {
        $.cookie("cart", JSON.stringify(this), {expires: 1800 }); 
    }
    GetCartCookie(): void {
        var cartCook = (<Cart>JSON.parse($.cookie("cart")));
        if (!cartCook) {
            return;
        }

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
            cart.AddToCart(itemId);
        });
    }
});