var MainPage = (function () {
    function MainPage() {
        this.caruselItem = $(".js-carusel-item");
    }
    MainPage.prototype.GetInfoBlock = function (id, type) {
        var _this = this;
        $.post('/Admin/ShowInfoBlockItem/', { id: id, type: type }).done(function (data) {
            _this.caruselItem.html(data);
        });
    };
    return MainPage;
}());
var mainPage;
$(function () {
    mainPage = new MainPage();
    $(document).on("click", ".js-show-carusel-item", function (e) {
        mainPage.GetInfoBlock($(e.currentTarget).data("id"), 0);
    });
});
//# sourceMappingURL=main-page.js.map