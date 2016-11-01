﻿$("#EditCartDialog").dialog({
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