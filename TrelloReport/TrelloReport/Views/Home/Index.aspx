<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrelloReport.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trello Weekly Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <%= Model.Message %></h1>
    <div class="row">
        <a href="" id="trello-authorize" target="_blank">Bejelentkezés</a>
    </div>
    <div class="row">
        <div class="span5 form-horizontal">
            <h6> <span class="badge badge-info">1</span>
                Tábla:</h6>
            <select id="trello-boards" class="span3">
                <option value="" selected="selected">Válassz egyet</option>
            </select>
        </div>
    </div>
    <div class="row"></div>
    <div class="row">
        <div class="span3">
            <h6> <span class="badge badge-info">2</span>
                Lista:</h6>
            <label class="checkbox">
                <input type="checkbox" checked="checked" id="board-lists-selectall" />Összes</label>
            <div id="board-lists">
            </div>
        </div>
        <div class="span3">
            <h6> <span class="badge badge-info">3</span>
                Felhasználó:</h6>
            <label class="checkbox">
                <input type="checkbox" checked="checked" id="board-users-selectall" />Összes</label>
            <div id="board-users">
            </div>
        </div>
        <div class="span3">
            <h6> <span class="badge badge-info">4</span>
                Típus:</h6>
            <div id="tab" class="btn-group" data-toggle="buttons-radio">
                <a href="#weekly" id="report-type-weekly" class="btn active" data-toggle="tab">Heti</a>
                <a href="#daily" id="report-type-daily" class="btn" data-toggle="tab">Napi</a>
                <input type="hidden" id="report-type" name="report-type" value="weekly" />
            </div>
            <p>
            </p>
            <div class="input-append">
                <input type="text" id="report-week" value="" maxlength="2" class="input-mini" />
                <span class="add-on">. hét</span>
            </div>
            <p>
                <input type="date" name="report-week-preview" id="report-week-preview" class="datepicker input-medium"
                    data-date-format="yyyy-mm-dd" readonly="readonly" disabled="disabled" /></p>
        </div>
        <div class="span3">
            <h6> <span class="badge badge-info">5</span>
                Riport:</h6>
            <p>
                <button type="button" id="generate-excel-report" class="btn btn-primary" disabled="disabled">
                    Excel riport készítés
                </button>
            </p>
            <p>
                <button type="button" id="generate-word-report" class="btn btn-primary" disabled="disabled">
                    Word riport készítés
                </button>
            </p>
            <p>
                <button type="button" id="generate-preview-report" class="btn " disabled="disabled">
                    Előnézet
                </button>
            </p>
        </div>
    </div>
    <div class="row">
        <div id="preview-data"></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            IsAuthenticated();

            weekNumberSetDefault();
        });

        $("#trello-boards").change(function () {
            var board = getSelectedBoard();
            FillLists(board);
            FillUsers(board);
        });

        $("button#generate-preview-report").click(function () {
            ReportPreview();
        });

        $("#report-week").keyup(function () {
            recalculateWeekStartDay();
        });

        $("#report-week").blur(function () {
            recalculateWeekStartDay();
        });

        $("a#report-type-weekly").click(function () {
            reportTypeWeeklySelect();
        });

        $("a#report-type-daily").click(function () {
            reportTypeDailySelect();
        });

        $('.datepicker').datepicker();

        $("#board-lists-selectall").click(function () {
            var checkedStatus = this.checked;
            $('div#board-lists').find('label :checkbox').each(function () {
                $(this).prop('checked', checkedStatus);
            });
        });

        $("#board-users-selectall").click(function () {
            var checkedStatus = this.checked;
            $('div#board-users').find('label :checkbox').each(function () {
                $(this).prop('checked', checkedStatus);
            });
        });

    </script>
</asp:Content>
