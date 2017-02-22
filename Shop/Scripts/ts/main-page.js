var MainPage = (function () {
    function MainPage() {
        this.caruselItem = $(".js-carusel-item");
        this.infoBlockItem = $(".js-info-block-item");
    }
    MainPage.prototype.getInfoBlock = function (id, type) {
        var _this = this;
        $.post('/Admin/ShowInfoBlockItem/', { id: id, type: type }).done(function (data) {
            switch (type) {
                case 0:
                    _this.caruselItem.html(data);
                    break;
                case 1:
                    _this.infoBlockItem.html(data);
                    break;
            }
        });
    };
    return MainPage;
}());
var mainPage;
$(function () {
    mainPage = new MainPage();
    $(document).on("click", ".js-show-info-block-item", function (e) {
        mainPage.getInfoBlock($(e.currentTarget).data("id"), $(e.currentTarget).data("type"));
    });
});
//# sourceMappingURL=main-page.js.map