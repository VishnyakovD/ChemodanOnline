class TopMenu {

    menu: JQuery;
    constructor() {
        this.menu = $(".js-top-menu-body-mobile");
    }

    Show(): void {
        this.menu.toggleClass("toggled");
        // this.menu.slideDown("slow");
    }

    Hide(): void {
        this.menu.removeClass("toggled");
    }
}

var topMenu: TopMenu;

$(() => {
    topMenu = new TopMenu();

    $(document).on("click", ".js-click-top-menu-btn", (e) => {
        if (!topMenu.menu.hasClass("toggled")) {
            topMenu.Show();
        } else {
            topMenu.Hide();
        }
    });


});