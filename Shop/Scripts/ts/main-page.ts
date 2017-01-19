class MainPage {
    caruselItem: JQuery;

    constructor() {
        this.caruselItem = $(".js-carusel-item");
    }

    GetInfoBlock(id:number, type:number):void {
        $.post('/Admin/ShowInfoBlockItem/', { id: id, type: type}).done((data) => {
            this.caruselItem.html(data);
        });
    }
}

var mainPage: MainPage;

$(() => {
    mainPage = new MainPage();

    $(document).on("click", ".js-show-carusel-item", (e) => {
        
        mainPage.GetInfoBlock($(e.currentTarget).data("id"),0);
    });


});