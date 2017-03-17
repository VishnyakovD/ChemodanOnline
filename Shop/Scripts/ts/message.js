var MessageManager = (function () {
    function MessageManager() {
        this.message = $(".js-message");
    }
    MessageManager.prototype.showMessage = function (text) {
        var _this = this;
        this.message.html(text);
        this.message.show();
        this.message.fadeTo("fast", 0.9);
        setTimeout(function () {
            _this.message.hide();
            _this.message.html("");
        }, 900);
    };
    return MessageManager;
}());
var message;
$(function () {
    message = new MessageManager();
});
