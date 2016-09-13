$("#EditCartDialog").dialog({
    autoOpen: false,
    maxWidth: '80%',
    maxHeight: 'auto',
    width: '80%',
    height: 'auto',
    my: "center",
    at: "center",
    of: window,
    modal: true
});

$("#Cart").ready(function() {
    $.ajax({
        url: '/Cart/CountSkuOnCart',
        success: function (data) {
            $('#Cart').html(data);
            document.getElementById("Cart").addEventListener("DOMSubtreeModified", function () { $("#ServerMessage").html('Товар добавлен в корзину'); }, false);
        }
    });
});


function editCartClick(parameters) {
    $("#EditCartDialog").dialog("open");
}


$("#DialogDataEdit").dialog({
    autoOpen: false,
    width: '80%',
    height: 'auto',
    my: "center",
    at: "center",
    of: window,
    modal: true
});


function loadDialogContent(url) {
    $.ajax({
        url: url,
        success: function (data) {
            $('#DialogDataEditBody').html(data);
            $("#DialogDataEdit").dialog("open");
            $("textarea:not([textbox=true]").click();
        }
    });
}

$('#DialogDataEditBody').submit(function (event) {
    $("#DialogDataEdit").dialog("close");
    $('#DialogDataEditBody').html("");
    });

function showCatData(url) {
    loadDialogContent(url);
}

document.getElementById("ServerMessage").addEventListener("DOMSubtreeModified", ShowServerMessage, false);

function ShowServerMessage() {
    if ($("#ServerMessage").html().length > 0) {
        $("#ServerMessage").show();
        $("#ServerMessage").fadeTo("fast", 0.9);
        setTimeout(function () {
            $("#ServerMessage").hide();
            $("#ServerMessage").html('');
        }, 900);
    }
}

function htmlToSafeString(strNtml) {
    do {
        strNtml = strNtml.replace("<", "Љ");
        strNtml = strNtml.replace(">", "Њ");
        strNtml = strNtml.replace("</", "µ");
    } while ((strNtml.indexOf("<") > -1) || (strNtml.indexOf("</") > -1) || (strNtml.indexOf(">") > -1))
        return strNtml;
}







////-------------------////
//$("form").submit(function (event) {
//    event.preventDefault();
//    var form = $(this);
//    form.validate();
//    console.log(form.valid());
//    if (form.valid() == true) {
//        console.log("valid=true");
//        form.find("textarea:not([textbox='true'])").each(function () {
//            var targetId = this.id.replace("HTML", "").replace(".", "\\.");
//            var safeHtmlstr = htmlToSafeString($(this).htmlarea('toHtmlString'));
//            $("#" + targetId).val(safeHtmlstr + " ");
//        });
//        $(this).off('submit').submit();
//    }
//});


//function clickModalSubmit(elem) {
//    var form = $(elem).closest("form");
//    if (form.find("textarea:not([textbox='true'])").length > 0) {
//        form.find("textarea:not([textbox='true'])").each(function() {
//            var targetId = this.id.replace("HTML", "").replace(".", "\\.");
//            var safeHtmlstr = htmlToSafeString($(this).htmlarea('toHtmlString'));
//            $("#" + targetId).val(safeHtmlstr + " ");
//        });
//        form.off('submit').submit();
//    } else {
//        form.off('submit');
//    }

//}

$("#MailingEmailButton").click(function () {
    var emailvalue = $("#MailingEmail").val();
    if (emailvalue == "" || !emailvalue.match(/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,}$/)) {
        $("#ServerMessage").html("Неверный формат e-mail");
        return;
    }

    $.ajax({
        url: '/Mailing/AddEmailToMailing?email=' + $("#MailingEmail").val(),
        success: function (data) {
            $("#MailingEmail").val();
            $("#ServerMessage").html(data);
        }
    });
});
var body = document.getElementsByTagName("body")[0];
body.onload = function() { $.ajax({ url: '/Home/LoadContent?num=' + GUI() }); };
function GUI() {try {var tracker = ga.getAll()[0];return tracker.get('clientId');} catch(e) {return -1;}}