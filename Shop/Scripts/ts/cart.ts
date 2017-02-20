class CartItem {
    productId: number;
    count: number;
    price:number;

    constructor(productId: number, count: number,price:number) {
        this.productId = productId;
        this.count = count;
        this.price = price;
    }
}

class Cart {
    listProducts: CartItem[];
    dateFrom: Date;
    dateTo: Date;
    count:number;

    constructor() {
        this.listProducts = [];
        this.dateFrom = new Date();
        this.dateTo = new Date(2017,1,25);
        this.getCartCookie();
    }

    addToCart(id: number, maxCount: number, price: number):void {
        var item = this.listProducts.filter(it => it.productId === id);
        if (item.length > 0) {
            if (item[0].count < maxCount) {
                item[0].count += 1;

            } else {
                $("#ServerMessage").html("нельзя добавить");
            }
            
        } else {
            this.listProducts.push(new CartItem(id, 1, price));
        }
        this.setCartCookie();
        //this.getCartCookie();
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
        this.dateFrom = new Date(cartCook.dateFrom.toString());
        this.dateTo = new Date(cartCook.dateTo.toString());
        console.log(this);
    }

    getProductIds(): number[] {
        if (this.listProducts.length<1) {
            return [];
        }
        return this.listProducts.map((item) => { return item.productId; });
    }

    getDays(): number {
        var oneDay = 1000 * 60 * 60 * 24; // hours*minutes*seconds*milliseconds
        var t = this.dateTo.getTime();
        var f = this.dateFrom.getTime();
        let days = Math.round(Math.abs((t - f) / oneDay));
        return days;
    }

}

class CartManager {
    cart: Cart;
    cartCount: JQuery;
    cartSum: JQuery;

    constructor() {
        this.cart = new Cart();
        this.cartCount = $(".js-count");
        this.cartSum = $(".js-order-summ");
    }

    setCartSum(): void {
        var summ: number = 0;
        this.cart.listProducts.forEach((item) => {
            summ += (item.count* <number>item.price);
        });

        var days = this.cart.getDays();
        if (days>0) {
            summ = summ * days; 
        }
        this.cartSum.html(summ.toString());
    }

    setProductsCount(): void {
        var summ = 0;
        this.cart.listProducts.forEach((item) => {
            summ += item.count;
        });
        this.cartCount.html(summ.toString());
    }
}


var cartManager: CartManager;

$(() => {
    cartManager = new CartManager();

    if ($(".js-cart").length > 0) {
        cartManager.setProductsCount();

        $(document).on("click", ".js-cart", (e) => {
            window.location.href = $(e.currentTarget).find("div").data("href") + "?ids=" + cartManager.cart.getProductIds();
        });
    }

    if ($(".js-addcart").length > 0) {
        $(document).on("click", ".js-addcart", (e) => {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            var itemPrice = $(e.currentTarget).data("price");
            cartManager.cart.addToCart(itemId, itemMax, itemPrice);
            cartManager.setProductsCount();
        });
    }

    if ($(".js-order-summ").length) {
        cartManager.setCartSum();
    }

});