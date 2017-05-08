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
    MessageManager.prototype.showMessageWnd = function (text, link) {
        var btn = "<button type='button' onclick='message.closeMessageWnd();' class='close'><span>&times;</span></button>";
        if (link != null && link.length > 0) {
            btn = "<a class='close' href='" + link + "'><span>&times;</span></a>";
        }
        this.message.html(btn + text);
        this.message.show();
    };
    MessageManager.prototype.closeMessageWnd = function () {
        this.message.hide();
        this.message.html("");
    };
    return MessageManager;
}());
var message;
$(function () {
    message = new MessageManager();
});
