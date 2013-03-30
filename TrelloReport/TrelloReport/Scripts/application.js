﻿/// <reference path="jquery-1.9.1-vsdoc.js" />

var isConsoleLogging = true;
function logError(error) {
    if (isConsoleLogging == true) {
        console.log(error);
    }
    else {
        alert(error);
    }
}

var urlIsAuthenticated = "/api/isauthenticated";
var urlFillBoards = "/api/getboards";
var urlFillLists = "/api/getlists";


function IsAuthenticatedSucces(result) {
    if (result.isLogged == true) {
        // logged in
        $("a#trello-authorize").hide();
        FillBoards();
    } else {
        // not logged in
        $("a#trello-authorize").attr("href", result.authUrl);
    }
};

function FillBoardsSucces(result) {
    $.each(result, function () {
        var options = $("select#trello-boards");
        options.append($("<option />").val(this.Id).text(this.Name));
    });
};

function FillListsSucces(result) {
    alert(result);
}

function IsAuthenticated() {
    var data = null;
    var request = $.ajax({
        url: urlIsAuthenticated,
        type: "POST",
        data: data,
        dataType: "json"
    });

    request.done(function (result) {
        IsAuthenticatedSucces(result);
    });

    request.fail(function (jqXHR, textStatus) {
        logError(textStatus);
    });
}

function FillBoards() {
    var data = null;
    var request = $.ajax({
        url: urlFillBoards,
        type: "POST",
        data: data,
        dataType: "json"
    });

    request.done(function (result) {
        FillBoardsSucces(result);
    });

    request.fail(function (jqXHR, textStatus) {
        logError(textStatus);
    });
}

function FillLists(boardId) {
    var data = { boardId: boardId };
    var request = $.ajax({
        url: urlFillLists,
        type: "POST",
        data: data,
        dataType: "json"
    });

    request.done(function (result) {
        FillListsSucces(result);
    });

    request.fail(function (jqXHR, textStatus) {
        logError(textStatus);
    });
}