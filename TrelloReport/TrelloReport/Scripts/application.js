/// <reference path="jquery-1.4.1-vsdoc.js" />

function IsAuthenticated(url) {
    $.ajax({
        type: "POST",
        url: url,
        data: "",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            if (result.isLogged == true) {
                // logged in
                $("a#trello-authorize").hide();
                FillBoards("/Api/GetBoards");
            } else {
                // not logged in
                $("a#trello-authorize").attr("href", result.authUrl);
            }
        },
        error: function (e) {
            alert(e);
        }
    });
}

function FillBoards(url) {
    $.ajax({
        type: "POST",
        url: url,
        data: "",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            $.each(result, function () {
                var options = $("select#trello-boadrs");
                options.append($("<option />").val(this.Id).text(this.Name));
            });
        },
        error: function (e) {
            alert(e);
        }
    });
}