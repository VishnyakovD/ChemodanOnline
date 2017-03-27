class MainPage {
    caruselItem: JQuery;
    infoBlockItem: JQuery;

    constructor() {
        this.caruselItem = $(".js-carusel-item");
        this.infoBlockItem = $(".js-info-block-item");
    }

    getInfoBlock(id:number, type:number):void {
        $.post('/Admin/ShowInfoBlockItem/', { id: id, type: type }).done((data) => {

            switch (type) {
                case 0:
                    this.caruselItem.html(data);
                    break;
                case 1:
                    this.infoBlockItem.html(data);
                    break;
            }
            
        });
    }
}

var mainPage: MainPage;

$(() => {
    mainPage = new MainPage();

    $(document).on("click", ".js-show-info-block-item", (e) => {
        mainPage.getInfoBlock($(e.currentTarget).data("id"), $(e.currentTarget).data("type"));
    });

    $(document).on("click", `a:not('[data-ajax="true"]')`, (e) => {
        document.location.href = $(e.currentTarget).attr("href");
    });


});