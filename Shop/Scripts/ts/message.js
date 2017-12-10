var MessageManager = (function () {
    function MessageManager() {
        this.message = $(".js-message");
        this.oneClickBody = "<div style='background: white' class='pay-one-click js-pay-one-click'><h5>Введите ваш телефон</h5><div class='line1'><input type='tel' placeholder='0XX XXX XX XX'></div><div class='line2'><div data-product-id='-1' onclick='payOneClick.payOneClick(event);'>Отправить заявку</div></div></div>";
    }
    MessageManager.prototype.showMessage = function (text, newMessage) {
        var _this = this;
        if (newMessage === void 0) { newMessage = null; }
        if (newMessage !== null) {
            var newMessageBody = this.message.clone();
            newMessageBody.html(text);
            $("body").append(newMessageBody);
            newMessageBody.closest(".js-glass").show();
            newMessageBody.show();
            newMessageBody.fadeTo("fast", 0.9);
            setTimeout(function () {
                newMessageBody.closest(".js-glass").remove();
                newMessageBody.remove();
            }, 900);
            return;
        }
        this.message.html(text);
        this.message.closest(".js-glass").show();
        this.message.show();
        this.message.fadeTo("fast", 0.9);
        setTimeout(function () {
            _this.message.hide();
            _this.message.html("");
            _this.message.closest(".js-glass").hide();
        }, 900);
    };
    MessageManager.prototype.showMessageWnd = function (text, link) {
        var btn = "<button type='button' onclick='message.closeMessageWnd();' class='close'><span>&times;</span></button>";
        var styles = "";
        if (link != null && link.length > 0) {
            btn = "<a class='close' href='" + link + "'><span>&times;</span></a>";
        }
        if (text === "oneclick") {
            text = this.oneClickBody;
            styles = "width:320px;left:42%;";
        }
        this.message.closest(".js-glass").show();
        this.message.html(btn + text);
        this.message.attr("style", styles);
        this.message.show();
    };
    MessageManager.prototype.closeMessageWnd = function () {
        this.message.closest(".js-glass").hide();
        this.message.hide();
        this.message.html("");
    };
    return MessageManager;
}());
var message;
var payOneClick;
$(function () {
    message = new MessageManager();
    payOneClick = new PayOneClick();
    //  	if ($(".js-pay-one-click").length > 0) {
    //}
});
//# sourceMappingURL=message.js.map