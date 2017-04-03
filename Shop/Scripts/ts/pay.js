var CardPay = (function () {
    function CardPay() {
    }
    CardPay.prototype.toString = function (param) {
        //if (param === "UnLockSumm") {
        //	return `${this.merchantAccount};${this.orderReference};${this.amount};${this.currency}`;
        //}
        //if (param === "PayLockSumm") {
        //	return `${this.merchantAccount};${this.orderReference};${this.amount};${this.currency}`;
        //}
        //if (param === "Pay") {
        //	return `${this.merchantAccount};${this.merchantDomainName};${this.orderReference};${this.orderDate};${this.amount};${this.currency};${this.productName};${this.productCount};${this.productPrice}`;
        //}
        if (param === "LockSumm") {
            return this.merchantAccount + ";" + this.merchantDomainName + ";" + this.orderReference + ";" + this.orderDate + ";" + this.amount + ";" + this.currency + ";" + this.productName + ";" + this.productCount + ";" + this.productPrice;
        }
        return this.merchantAccount + ";" + this.merchantDomainName + ";" + this.orderReference + ";" + this.orderDate + ";" + this.amount + ";" + this.currency + ";" + this.productName + ";" + this.productCount + ";" + this.productPrice;
    };
    return CardPay;
}());
$(document).ready(function () {
    var order = "WB000050006";
    var payBtn = $(".js-lock-summ-btn"); //блокировка суммы
    if (payBtn.length > 0) {
        var wayforpay = new Wayforpay();
        $(document).on("click", ".js-lock-summ-btn", function () {
            var dateTime = new Date();
            var ticks = ((dateTime.getTime() * 10000));
            var cardPay1 = new CardPay();
            cardPay1.merchantAccount = "test_merch_n1";
            cardPay1.merchantDomainName = "chemodan.online";
            cardPay1.authorizationType = "SimpleSignature";
            cardPay1.merchantSignature = "";
            cardPay1.orderReference = order;
            cardPay1.orderDate = ticks.toString();
            cardPay1.amount = "0.01";
            cardPay1.currency = "UAH";
            cardPay1.productName = "Оплата услуг по договору " + order;
            cardPay1.productPrice = "0.01";
            cardPay1.productCount = "1";
            cardPay1.clientFirstName = "Дмитрий";
            cardPay1.clientLastName = "Вишняков";
            cardPay1.clientEmail = "vishnyavd@gmail.com";
            cardPay1.clientPhone = "380932178000";
            cardPay1.holdTimeout = " 1728000"; //это 20 дней
            cardPay1.merchantTransactionType = "AUTH"; //блокировка суммы
            $.post("/order/GenerateSignature", { data: cardPay1.toString("LockSumm") }, function (data) {
                cardPay1.merchantSignature = data;
                wayforpay.run(cardPay1, function (response) {
                    console.log("approved");
                    console.log(response.recToken);
                }, function (response) {
                    // on declined.
                    console.log("declined");
                    console.log(response);
                }, function (response) {
                    // on pending or in processing
                    console.log("pending or in processing");
                    console.log(response);
                });
            });
        });
    }
});
