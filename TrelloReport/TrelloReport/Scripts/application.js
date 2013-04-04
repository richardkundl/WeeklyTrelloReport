/// <reference path="jquery-1.9.1-vsdoc.js" />
Date.prototype.getWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    return Math.ceil((((this - onejan) / 86400000) + onejan.getDay() + 1) / 7);
};

function toTwoDigitNumber(number) {
    if (number.toString().length > 1) {
        return number;
    }

    return '0' + number;
}

function startDateOfWeek(weekNo) {
    var d1 = new Date();
    var numOfdaysPastSinceLastMonday = eval(d1.getDay() - 1);
    d1.setDate(d1.getDate() - numOfdaysPastSinceLastMonday);
    var weekNoToday = d1.getWeek();
    var weeksInTheFuture = eval(weekNo - weekNoToday);
    d1.setDate(d1.getDate() + eval(7 * weeksInTheFuture));
    var rangeIsFrom = d1.getFullYear() + "-" + toTwoDigitNumber(eval(d1.getMonth() + 1)) + "-" + toTwoDigitNumber(d1.getDate());
    return rangeIsFrom;
};

function endDateOfWeek(weekNo) {
    var d1 = new Date();
    var numOfdaysPastSinceLastMonday = eval(d1.getDay() - 1);
    d1.setDate(d1.getDate() - numOfdaysPastSinceLastMonday);
    var weekNoToday = d1.getWeek();
    var weeksInTheFuture = eval(weekNo - weekNoToday);
    d1.setDate(d1.getDate() + eval(7 * weeksInTheFuture));
    d1.setDate(d1.getDate() + 6);
    var rangeIsTo = d1.getFullYear() + "-" + toTwoDigitNumber(eval(d1.getMonth() + 1)) + "-" + toTwoDigitNumber(d1.getDate());
    return rangeIsTo;
};

var isConsoleLogging = true;
function logError(error) {
    if (isConsoleLogging == true) {
        console.log(error);
    }
    else {
        alert(error);
    }
}

function weekNumberSetDefault() {
    var week = $("#report-week").val();

    if (week == '') {
        week = (new Date()).getWeek();
        $("#report-week").val(week);
    }

    $("#report-week").blur();
}

function recalculateWeekStartDay() {
    var week = $("#report-week").val();

    if (week == NaN) {
        alert("A riportolni kivánt hét nem szám!");
    }

    $("input#report-week-preview").val(startDateOfWeek(week));
}

function reportTypeWeeklySelect() {
    $("input#report-type").val("weekly");
    $("input#report-week").parent().show();
    $("input#report-week-preview").attr('readonly', 'readonly');
    $("input#report-week-preview").attr('disabled', 'disabled');
    recalculateWeekStartDay();
}

function reportTypeDailySelect() {
    $("input#report-type").val("daily");
    $("input#report-week").parent().hide();
    $("input#report-week-preview").removeAttr("readonly");
    $("input#report-week-preview").removeAttr("disabled");
}

function reportButtonsEnabled() {
    $('button#generate-excel-report').removeAttr('disabled');
    $('button#generate-word-report').removeAttr('disabled');
    $('button#generate-preview-report').removeAttr('disabled');
}

function reportButtonsDisabled() {
    $('button#generate-excel-report').attr('disabled', 'disabled');
    $('button#generate-word-report').attr('disabled', 'disabled');
    $('button#generate-preview-report').attr('disabled', 'disabled');
}

function getSelectedBoard() {
    var board = $("#trello-boards").find(":selected").val();
    return board;
}

function getReportTypeInterval() {
    var interval = $("input#report-type").val();
    return interval;
}

function getReportStartDate() {
    var startDate = $("input#report-week-preview").val();
    return startDate;
}

function getCheckdItems(selector) {
    var names = [];
    $(selector).each(function () {
        names.push(this.value);
    });
    return names;
}

var urlIsAuthenticated = "/api/isauthenticated";
var urlFillBoards = "/api/getboards";
var urlFillLists = "/api/getlists";
var urlFillUsers = "/api/getusersonboard";
var urlReportPreview = "/api/reportpreview";


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
        var container = $("select#trello-boards");
        container.append($("<option />").val(this.Id).text(this.Name));
    });
};

function FillListsClean() {
    var container = $('div#board-lists');
    container.html('');
    reportButtonsDisabled();
}

function FillListsSucces(result) {
    var container = $('div#board-lists');
    container.html('');
    $.each(result, function () {
        var label = $('<label />', { class: 'checkbox', text: this.Name }).appendTo(container);
        $('<input />', { type: 'checkbox', name: 'boardListsCb', value: this.Id, checked: 'checked' }).appendTo(label);
    });
    reportButtonsEnabled();
}

function FillUsersClean() {
    var container = $('div#board-users');
    container.html('');
    reportButtonsDisabled();
}

function FillUsersSucces(result) {
    var container = $('div#board-users');
    container.html('');
    $.each(result, function () {
        var label = $('<label />', { class: 'checkbox', text: this.FullName }).appendTo(container);
        $('<input />', { type: 'checkbox', name: 'boardUsersCb', value: this.Id, checked: 'checked' }).appendTo(label);
    });
    reportButtonsEnabled();
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

function FillUsers(boardId) {
    FillUsersClean();
    if (boardId == '') {
        return;
    }

    var data = { boardId: boardId };
    var request = $.ajax({
        url: urlFillUsers,
        type: "POST",
        data: data,
        dataType: "json"
    });

    request.done(function (result) {
        FillUsersSucces(result);
    });

    request.fail(function (jqXHR, textStatus) {
        logError(textStatus);
    });
}

function FillLists(boardId) {
    FillListsClean();
    if (boardId == '') {
        return;
    }

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

function ReportPreview() {
    var boardId = getSelectedBoard();
    var listIds = getCheckdItems("div#board-lists input:checked");
    var userIds = getCheckdItems("div#board-users input:checked"); 
    var reportIntervalType = getReportTypeInterval();
    var startDate = getReportStartDate();

    var data = { boardId: boardId, listIds: JSON.stringify(listIds), userIds: JSON.stringify(userIds), 
                reportIntervalType: reportIntervalType, startDate: startDate };
    var request = $.ajax({
        url: urlReportPreview,
        type: "POST",
        data: data,
        dataType: "json"
    });

    request.done(function (result) {
        alert(result);
    });

    request.fail(function (jqXHR, textStatus) {
        logError(textStatus);
    });
}