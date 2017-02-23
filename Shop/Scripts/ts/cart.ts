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
    days:number;

    constructor() {
        this.listProducts = [];
        this.dateFrom = new Date();
        this.dateTo = new Date();
        this.setDays();
        this.getCartCookie();
    }

    addToCart(id: number, maxCount: number, price: number): number {
        var countResult: number;
        var item = this.listProducts.filter(it => it.productId === id);
        if (item.length > 0) {
            if (item[0].count < maxCount) {
                item[0].count += 1;
                countResult = item[0].count;
            } else {
                countResult = item[0].count;
                $("#ServerMessage").html("нельзя добавить");
            }
            
        } else {
            this.listProducts.push(new CartItem(id, 1, price));
            countResult = 1;
        }
        this.setCartCookie();
        return countResult;
    }

    removeFromCart(id: number): number {
        var countResult = 0;
        var item = this.listProducts.filter(item => item.productId === id);
        if (item.length > 0) {
            item[0].count -= 1;
            countResult = item[0].count;
            if (item[0].count<1) {
                this.listProducts = this.listProducts.filter(item => item.productId !== id);
                countResult = 0;
            }
        } 
        this.setCartCookie();
        return countResult;
    }

    setCartCookie(): void {
        var curDate = new Date();
        curDate.setMinutes(curDate.getMinutes() + 60);
        $.cookie("cart", JSON.stringify(this), { expires: curDate,path:"/" }); 
    }

    getCartCookie(): void {
        if (!$.cookie("cart")) 
            return;

        var cartCook = (<Cart>JSON.parse($.cookie("cart")));
        this.listProducts = cartCook.listProducts;
        this.dateFrom = new Date(cartCook.dateFrom.toString());
        this.dateTo = new Date(cartCook.dateTo.toString());
    }

    getProductIds(): number[] {
        if (this.listProducts.length<1) {
            return [];
        }
        return this.listProducts.map((item) => { return item.productId; });
    }

    setDays(): void {
        var t = this.dateTo.getTime();
        var f = this.dateFrom.getTime();
        var days: number = Math.round(Math.abs((t - f) / 86400000));
        if (days<1) {
            days = 1;
        }
        this.days = days;
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

    setCountOneProductInOrderPage(curent: JQuery,count:number): void {
        var countControl = curent.closest(".js-control-count");
        if (countControl.length > 0) {
            countControl.find(".js-count-item").html(count.toString());
        }
    }

    setSummOneProductInOrderPage(id: JQuery, price: number, count: number): void {
        var page1 = $(".js-order-page1");
        if (page1.length > 0) {
            page1.find(`.js-summ-item[data-id=${id}]`).html((price*count*this.cart.days).toString());
        }
    }

    setCountAllProducts(): void {
        var page1 = $(".js-order-page1");
        if (page1.length>0) {
            this.cart.listProducts.forEach(item => {
                page1.find(`.js-count-item[data-id=${item.productId}]`).html(item.count.toString());
            });
        }
    }

    setSummAllProducts(): void {
        var page1 = $(".js-order-page1");
        if (page1.length > 0) {
            this.cart.listProducts.forEach(item => {
                var summ = item.count * item.price * this.cart.days;
                page1.find(`.js-summ-item[data-id=${item.productId}]`).html(summ.toString());
            });
        }
    }

    setCartSum(): void {
        if (this.cartSum.length>0) {
            var summ: number = 0;
            this.cart.listProducts.forEach((item) => {
                summ += (item.count * item.price);
            });
            
            summ = summ * this.cart.days;
            this.cartSum.html(summ.toString()); 
        }
    }

    setProductsCount(): void {
        if (this.cartCount.length>0) {
            var summ = 0;
            this.cart.listProducts.forEach((item) => {
                summ += item.count;
            });
            this.cartCount.html(summ.toString());
        }
    }
}

var cartManager: CartManager;

$(() => {
    cartManager = new CartManager();
    cartManager.setCountAllProducts();
    cartManager.setSummAllProducts();
    if ($(".js-cart").length > 0) {
        cartManager.setProductsCount();

        $(document).on("click", ".js-cart", (e) => {
            var location = `${$(e.currentTarget).find("div").data("href")}?ids=${cartManager.cart.getProductIds()}`;
            window.location.href = location;
        });
    }

    if ($(".js-addcart").length > 0) {
        $(document).on("click", ".js-addcart", (e) => {
            var itemId = $(e.currentTarget).data("id");
            var itemMax = $(e.currentTarget).data("max");
            var itemPrice = $(e.currentTarget).data("price");
            var count = cartManager.cart.addToCart(itemId, itemMax, itemPrice);

            cartManager.setProductsCount();
            cartManager.setCountOneProductInOrderPage($(e.currentTarget), count);
            cartManager.setSummOneProductInOrderPage(itemId, itemPrice,count);
            cartManager.setCartSum();
        });
    }

    if ($(".js-removecart").length > 0) {
        $(document).on("click", ".js-removecart", (e) => {
            var itemId = $(e.currentTarget).data("id");
            var itemPrice = $(e.currentTarget).data("price");
            var count = cartManager.cart.removeFromCart(itemId);

            cartManager.setProductsCount();
            cartManager.setCountOneProductInOrderPage($(e.currentTarget), count);
            cartManager.setSummOneProductInOrderPage(itemId, itemPrice,count);
            cartManager.setCartSum();
        });
    }

    if ($(".js-order-summ").length>0) {
        cartManager.setCartSum();
    }

});