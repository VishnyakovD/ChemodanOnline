class InputErrorItem {
    inputName: string;
    inputError: string;

    constructor(inputName: string, inputError: string) {
        this.inputName = inputName;
        this.inputError = inputError;
    }
}

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
    from: Date;
    to: Date;
    clientLastName:string;
    clientFirstName: string;
    clientEmail: string;
    clientPhone: string;
    city: string;
    home: string;
    typeStreet: string;
    level: string;
    street: string;
    flat: string;
    deliveryType:number;
    paymentType: number;



    listProducts: CartItem[];
    count:number;
    days: number;


    constructor() {
        var date = new Date();
        this.listProducts = [];
        this.from = new Date(date.setHours(0, 0, 0, 0));
        this.to = new Date(date.setHours(23, 59, 59, 99));
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
                message.showMessage("товар добавлен в корзину");
            } else {
                countResult = item[0].count;
                message.showMessage("нельзя добавить");
            }
            
        } else {
            this.listProducts.push(new CartItem(id, 1, price));
            message.showMessage("товар добавлен в корзину");
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
        this.from = new Date(cartCook.from.toString());
        this.to = new Date(cartCook.to.toString());
    }

    getProductIds(): number[] {
        if (this.listProducts.length<1) {
            return [];
        }
        return this.listProducts.map((item) => { return item.productId; });
    }

    setDays(): void {
        var t = this.to.getTime();
        var f = this.from.getTime();
        var days: number = Math.ceil(((t - f) / 86400000));
        if (days<1) {
            days = 1;
        }
        this.days = days;
    }

    setDatesFromAndTo(from:any,to:any): void {
        this.from = new Date(new Date(from).setHours(0, 0, 0, 0));
        this.to = new Date(new Date(to).setHours(23, 59, 59, 99));
        this.setDays();
        this.setCartCookie();
    }

    validate():InputErrorItem [] {
        var arrors = [];

        if (this.from.toString() === "Invalid Date" || this.from.setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0) ) {
            arrors.push(new InputErrorItem("From", ""));
        }

        var dateFrom = new Date(this.from.toDateString());
        if (this.to.toString() === "Invalid Date" || this.to.setHours(23, 59, 59, 99) <= dateFrom.setHours(23, 59, 59, 99)) {
            arrors.push(new InputErrorItem("To", ""));
        }

        if (this.clientEmail === "" || !this.clientEmail.match(/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,}$/)) {
            arrors.push(new InputErrorItem("ClientEmail", ""));
        }
        if (this.clientFirstName === "") {
            arrors.push(new InputErrorItem("ClientFirstName", ""));
        }
        if (this.clientLastName === "") {
            arrors.push(new InputErrorItem("ClientLastName", ""));
        }
        if (this.clientPhone === "") {
            arrors.push(new InputErrorItem("ClientPhone", ""));
        }
        if (this.paymentType < 1) {
            arrors.push(new InputErrorItem("PaymentType", ""));
        }
        if (this.deliveryType === 2 || this.deliveryType === 3) {
            //arrors.push(new InputErrorItem("DeliveryType", ""));
            if (this.city === "") {
                arrors.push(new InputErrorItem("City", ""));
            }
            if (this.flat === "") {
                arrors.push(new InputErrorItem("Flat", ""));
            }
            if (this.home === "") {
                arrors.push(new InputErrorItem("Home", ""));
            }
            if (this.typeStreet === "") {
                arrors.push(new InputErrorItem("TypeStreet", ""));
            }
            if (this.level === "") {
                arrors.push(new InputErrorItem("Level", ""));
            }

            if (this.street === "") {
                arrors.push(new InputErrorItem("Street", ""));
            }
        }
        return arrors;
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
        this.cart.from = new Date(date.setHours(0, 0, 0, 0));
        this.cart.setDays();
        this.setCartSum();
        this.setCountAllProducts();
        this.setSummAllProducts();
      //  $(".js-card-dates input[name=From]").val(`${this.cart.from.toDateString()}`);
    } 
       
    setDateTo(event:any):void {
        var date = new Date(event.target.value);
        this.cart.to = new Date(date.setHours(23, 59, 59, 99));
        this.cart.setDays();
        this.setCartSum();
        this.setCountAllProducts();
        this.setSummAllProducts();
      //  $(".js-card-dates input[name=To]").val(this.cart.to.toString());
    }

    validateCart():boolean {
        var errors = this.cart.validate();
        var orderPages = $(".js-order-pages");
        orderPages.find(".error").removeClass("error");
        errors.forEach(item => {
            orderPages.find(`[name=${item.inputName}]`).addClass("error");
        });
        if (errors.length>0) {
            return false;
        }
        return true;
    }

    visibilityAddres(visible:boolean):void {
        var orderPage = $(".js-order-pages");
        if (visible) {
            orderPage.find(".js-addres").removeClass("hide");
        } else {
            orderPage.find(".js-addres").addClass("hide");
        }
        
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
                message.showMessage("ваша корзина пуста");
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

            var count = 0;
            if (itemRemoveAll) {
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
            var orderPage = $(".js-order-pages");
            if (orderPage.length>0) {
                cartManager.cart.clientLastName = orderPage.find("[name=ClientLastName]").val();
                cartManager.cart.clientFirstName = orderPage.find("[name=ClientFirstName]").val();
                cartManager.cart.clientEmail = orderPage.find("[name=ClientEmail]").val();
                cartManager.cart.clientPhone = orderPage.find("[name=ClientPhone]").val();
                cartManager.cart.city = orderPage.find("[name=City]").val();
                cartManager.cart.home = orderPage.find("[name=Home]").val();
                cartManager.cart.typeStreet = orderPage.find("[name=TypeStreet]").val();
                cartManager.cart.level = orderPage.find("[name=Level]").val();
                cartManager.cart.street = orderPage.find("[name=Street]").val();
                cartManager.cart.flat = orderPage.find("[name=Flat]").val();
                cartManager.cart.deliveryType = parseInt(orderPage.find("[name=DeliveryType]").val());
                cartManager.cart.paymentType = parseInt(orderPage.find("[name=PaymentType]").val());
                if (cartManager.validateCart()) {
                    $.post("/Order/CreateOrder/", { order: JSON.stringify(cartManager.cart) })
                        .done(result => {
                            message.showMessage(result);
                            //если заказ создан нужно почистить куки и показать всплывающее окно с крестиком
                        });
                }
            }
        });
    }

    if ($(".js-delivery-type").length>0) {
        $(document).on("click", ".js-delivery-type", (e) => {
            if ($(e.currentTarget).data("id") === 2 || $(e.currentTarget).data("id") === 3) {
                cartManager.visibilityAddres(true);
            } else {
                cartManager.visibilityAddres(false); 
            }
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