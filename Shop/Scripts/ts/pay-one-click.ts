class PayOneClick {
    constructor() {}
    payOneClick(e) {
        var phone = $(e.currentTarget).closest(".js-pay-one-click").find("input");
        var productId = $(e.currentTarget).data("product-id");


        var date = new Date();
        var stringDate = date.toLocaleDateString() + " " + date.toLocaleTimeString();

        $.post("/Order/PayOneClick/", { phone: phone.val(), productId: productId, createDate: stringDate})
            .done(result => {
                message.showMessage(result);
                phone.val("");
                //если заказ создан нужно почистить куки и показать всплывающее окно с крестиком
            });
    }
}

var payOneClick: PayOneClick;

$(() => {
    if ($(".js-pay-one-click").length>0) {
          payOneClick = new PayOneClick();  
    }
});