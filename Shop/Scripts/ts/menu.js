var TopMenu = (function () {
    function TopMenu() {
        this.menu = $(".js-top-menu-body-mobile");
        this.menuBig = $(".js-menu-body a");
        this.menuBig.each(function (i, element) {
            if (window.location.href.indexOf($(element).attr("href")) > 0) {
                $(element).addClass("active");
                return;
            }
        });
    }
    TopMenu.prototype.show = function () {
        this.menu.toggleClass("toggled");
        // this.menu.slideDown("slow");
    };
    TopMenu.prototype.hide = function () {
        this.menu.removeClass("toggled");
    };
    return TopMenu;
}());
var topMenu;
$(function () {
    topMenu = new TopMenu();
    $(document).on("click", ".js-click-top-menu-btn", function (e) {
        if (!topMenu.menu.hasClass("toggled")) {
            topMenu.show();
        }
        else {
            topMenu.hide();
        }
    });
});
//# sourceMappingURL=menu.js.map