

class CartItem {
    productId: number;
    count: number;

    constructor(productId: number, count: number) {
        this.productId =productId;
        this.count = count;
    }
}

class Cart {
    listProducts: CartItem[];
    listIds:number[];
    dateFrom: Date;
    dateTo: Date;

    constructor() {
        this.listProducts = [];
        this.listIds = [];
        this.dateFrom = new Date();
        this.dateTo = new Date();
        this.getCartCookie();
    }



    addToCart(id: number, maxCount: number):void {
        var item = this.listProducts.filter(item => item.productId === id);
        if (item.length > 0) {
            if (item[0].count < maxCount) {
                item[0].count += 1;
            } else {
                $("#ServerMessage").html("нельзя добавить");
            }
            
        } else {
            this.listProducts.push(new CartItem(id, 1));
        }
        this.setCartCookie();
        this.getCartCookie();
    }

    removeFromCart(id: number): void {//не проверил
        var item = this.listProducts.filter(item => item.productId === id);
        if (item.length > 0) {
            item[0].count -= 1;

            if (item[0].count<1) {
                this.listProducts = this.listProducts.filter(item => item.productId !== id);
            }
        } 

        this.setCartCookie();
        this.getCartCookie();
    }

    setCartCookie(): void {
        var curDate = new Date();
        curDate.setMinutes(curDate.getMinutes() + 60);
        $.cookie("cart", JSON.stringify(this), { expires: curDate,path:"/" }); 
    }
    getCartCookie(): void {

        if (!$.cookie("cart")) {
            return;
        }
        var cartCook = (<Cart>JSON.parse($.cookie("cart")));
        
        this.listProducts = cartCook.listProducts;
        this.dateFrom = cartCook.dateFrom;
        this.dateTo = cartCook.dateTo;
        console.log(this);
    }

    getProductIds(): number[] {
        if (this.listProducts.length<1) {
            return [];
        }
        return this.listProducts.map((item) => { return item.productId; });
    }

}


var cart: Cart;

$(() => {
    cart = new Cart();

    if ($(".js-addcart").length>0) {
        $(document).on("click", ".js-addcart", (e) => {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            cart.addToCart(itemId, itemMax);
        });
    }

    if ($(".js-cart").length>0) {
        $(document).on("click", ".js-cart", (e) => {
            window.location.href = $(e.currentTarget).find("div").data("href") + "?ids=" + cart.getProductIds();
        });
    }
});