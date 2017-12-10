class MessageManager {
	message: JQuery;
	oneClickBody: string;

    constructor() {
		this.message = $(".js-message");
		this.oneClickBody = "<div style='background: white' class='pay-one-click js-pay-one-click'><h5>Введите ваш телефон</h5><div class='line1'><input type='tel' placeholder='0XX XXX XX XX'></div><div class='line2'><div data-product-id='-1' onclick='payOneClick.payOneClick(event);'>Отправить заявку</div></div></div>";
    }

    showMessage(text: string, newMessage:any=null): void {
		if (newMessage !== null) {
			var newMessageBody = this.message.clone();
			newMessageBody.html(text);
			$("body").append(newMessageBody);
			newMessageBody.closest(".js-glass").show();
			newMessageBody.show();
			newMessageBody.fadeTo("fast", 0.9);
			
			setTimeout(() => {
				newMessageBody.closest(".js-glass").remove();
				newMessageBody.remove();
			}, 900);

			return;
		}
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

    showMessageWnd(text: any, link:string): void {
		var btn = "<button type='button' onclick='message.closeMessageWnd();' class='close'><span>&times;</span></button>";
		var styles = "";
        if (link!=null && link.length>0) {
            btn = "<a class='close' href='" + link + "'><span>&times;</span></a>";
		}



		if (text === "oneclick") {
			text = this.oneClickBody;
			styles = "width:320px;left:42%;";
		}
        this.message.closest(".js-glass").show();
		this.message.html(btn + text);
		this.message.attr("style", styles);
        this.message.show();
    }

    closeMessageWnd(): void {
        this.message.closest(".js-glass").hide();
        this.message.hide();
        this.message.html("");
    }
}

var message: MessageManager;
var payOneClick: PayOneClick;
$(() => {
	message = new MessageManager();
payOneClick = new PayOneClick();
 //  	if ($(".js-pay-one-click").length > 0) {
		
	//}
});

