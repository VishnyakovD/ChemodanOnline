class TopMenu {

    menu: JQuery;
    menuBig:JQuery;
    constructor() {
        this.menu = $(".js-top-menu-body-mobile");
        this.menuBig = $(".js-menu-body a");

        this.menuBig.each((i, element) => {
            console.log(element);
            if (window.location.href.indexOf($(element).attr("href")) > 0) {
                $(element).addClass("active");
                return;
            }
        });
    }

    show(): void {
        this.menu.toggleClass("toggled");
        // this.menu.slideDown("slow");
    }

    hide(): void {
        this.menu.removeClass("toggled");
    }
}

var topMenu: TopMenu;

$(() => {
    topMenu = new TopMenu();

    $(document).on("click", ".js-click-top-menu-btn", (e) => {
        if (!topMenu.menu.hasClass("toggled")) {
            topMenu.show();
        } else {
            topMenu.hide();
        }
    });


});