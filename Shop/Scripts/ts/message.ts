class MessageManager {
    message: JQuery;

    constructor() {
        this.message = $(".js-message");
    }

    showMessage(text: string): void {
        this.message.html(text);
        this.message.show();
            this.message.fadeTo("fast", 0.9);
            setTimeout(() => {
                this.message.hide();
                this.message.html("");
            }, 900);
    }
}

var message: MessageManager;

$(() => {
    message = new MessageManager();
   
});