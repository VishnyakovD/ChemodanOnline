class MessageManager {
    message: JQuery;

    constructor() {
        this.message = $(".js-message");
    }

    showMessage(text: string): void {
        this.message.html(text);
        this.message.closest(".js-glass").show();
        this.message.show();
            this.message.fadeTo("fast", 0.9);
            setTimeout(() => {
                this.message.hide();
                this.message.html("");
                this.message.closest(".js-glass").hide();
            }, 900);
    }

    showMessageWnd(text: string, link:string): void {
        var btn = "<button type='button' onclick='message.closeMessageWnd();' class='close'><span>&times;</span></button>";
        if (link!=null && link.length>0) {
            btn = "<a class='close' href='" + link + "'><span>&times;</span></a>";
        }
        this.message.closest(".js-glass").show();
        this.message.html(btn+text);
        this.message.show();
    }

    closeMessageWnd(): void {
        this.message.closest(".js-glass").hide();
        this.message.hide();
        this.message.html("");
    }
}

var message: MessageManager;

$(() => {
    message = new MessageManager();
   
});