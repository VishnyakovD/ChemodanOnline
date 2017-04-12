class PayOneClick {
    constructor() {}
    payOneClick(e) {
        var phone = $(e.currentTarget).closest(".js-pay-one-click").find("input");
        var productId = $(e.currentTarget).data("product-id");


        var date = new Date();
        var stringDate = date.toLocaleDateString() + " " + date.toLocaleTimeString();

        $.post("/Order/PayOneClick/", { phone: phone.val(), productId: productId, createDate: stringDate})
            .done(result => {
                phone.val("");
                $.removeCookie('cart', { path: '/' });
                message.showMessageWnd(result, "/");
            });
    }
}

var payOneClick: PayOneClick;

$(() => {
    if ($(".js-pay-one-click").length>0) {
          payOneClick = new PayOneClick();  
    }
});