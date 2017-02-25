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
        var date = new Date();
        this.listProducts = [];
        this.dateFrom = new Date(date.setHours(0, 0, 0, 0));
        this.dateTo = new Date(date.setHours(23, 59, 59, 99));
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
                $("#ServerMessage").html("товар добавлен в корзину");
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

    removeFromCart(id: number,removeAll:boolean): number {
        var countResult = 0;
        var item = this.listProducts.filter(item => item.productId === id);
        if (item.length > 0) {
            item[0].count -= 1;
            countResult = item[0].count;
            if (removeAll || item[0].count<1) {
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
        var days: number = Math.ceil(((t - f) / 86400000));
        if (days<1) {
            days = 1;
        }
        this.days = days;
    }

    setDatesFromAndTo(from:any,to:any): void {
        this.dateFrom = new Date(new Date(from).setHours(0, 0, 0, 0));
        this.dateTo = new Date(new Date(to).setHours(23, 59, 59, 99));
        this.setDays();
        this.setCartCookie();
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

    removeCartLineInOrderPage(curent: JQuery): void {
        var line = curent.closest(".js-cart-line");
        if (line.length > 0) {
            line.remove();
        }
    }

    setCountOneProductInOrderPage(curent: JQuery, count: number): void {
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
    
    setDateFrom(event:any):void {
        var date = new Date(event.target.value);
        this.cart.dateFrom = new Date(date.setHours(0, 0, 0, 0));
        this.cart.setDays();
        this.setCartSum();
        this.setCountAllProducts();
        this.setSummAllProducts();
    } 
       
    setDateTo(event:any):void {
        var date = new Date(event.target.value);
        this.cart.dateTo = new Date(date.setHours(23, 59, 59, 99));
        this.cart.setDays();
        this.setCartSum();
        this.setCountAllProducts();
        this.setSummAllProducts();
    }
}

var cartManager: CartManager;

$(() => {
    cartManager = new CartManager();
    if ($(".js-cart").length > 0) {
        cartManager.setProductsCount();

        $(document).on("click", ".js-cart", (e) => {
            if (cartManager.cart.listProducts.length > 0) {
                window.location.href = `${$(e.currentTarget).find("div").data("href")}?ids=${cartManager.cart.getProductIds()}`;
            } else {
                $("#ServerMessage").html("ваша корзина пуста");
            }
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
            var itemRemoveAll = $(e.currentTarget).data("remove-all");
            console.log(itemRemoveAll);
            var count = 0;
            if (itemRemoveAll == "true") {
                count = cartManager.cart.removeFromCart(itemId, true);
            } else {
                count = cartManager.cart.removeFromCart(itemId, false);
            }
            if (count===0) {
                cartManager.removeCartLineInOrderPage($(e.currentTarget));
            }
            cartManager.setProductsCount();
            cartManager.setCountOneProductInOrderPage($(e.currentTarget), count);
            cartManager.setSummOneProductInOrderPage(itemId, itemPrice,count);
            cartManager.setCartSum();
        });
    }

    if ($(".js-confirm-order").length>0) {
        $(document).on("click", ".js-confirm-order", (e) => {
            //ClientLastName
            //ClientFirstName
            //ClientEmail
            //ClientPhone
            //City
            //Home
            //TypeStreet
            //Level
            //Street
            //Flat
            //DeliveryType
            //PaymentType
            //
            //
            //

        });
    }

    if ($(".js-card-dates").length > 0) {
        var dates = $(".js-card-dates");
        cartManager.cart.setDatesFromAndTo(dates.find("input[name=From]").val(), dates.find("input[name=To]").val());
    }

    if ($(".js-order-summ").length > 0) {
        cartManager.setCartSum();
    }
    cartManager.setCountAllProducts();
    cartManager.setSummAllProducts();
});