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
        <div class="span3">
            <select id="trello-boards" class="span3">
                <option value="" selected="selected">Válassz egyet</option>
            </select>
        </div>
    </div>
    <div class="row">
        <div class="span3">
            <h6>
                Lista:</h6>
            <label class="checkbox">
                <input type="checkbox" checked="checked" id="board-lists-selectall" />Összes</label>
            <div id="board-lists">
            </div>
        </div>
        <div class="span3">
            <h6>
                Felhasználó:</h6>
            <label class="checkbox">
                <input type="checkbox" checked="checked" id="board-users-selectall" />Összes</label>
            <div id="board-users">
            </div>
        </div>
        <div class="span3">
            <h6>
                Típus:</h6>
            <div id="tab" class="btn-group" data-toggle="buttons-radio">
                <a href="#weekly" id="report-type-weekly" class="btn active" data-toggle="tab">Heti</a>
                <a href="#daily" id="report-type-daily" class="btn" data-toggle="tab">Napi</a>
                <input type="hidden" id="report-type" name="report-type" value="weekly" />
            </div>
            <p></p>
            <div class="input-append">
                <input type="text" id="report-week" value="" maxlength="2" class="input-mini" />
                <span class="add-on">. hét</span>
            </div>
            <p>
                <input type="date" name="report-week-preview" id="report-week-preview" class="input-medium"
                    data-date-format="yy.mm.dd" readonly="readonly" /></p>
        </div>
        <div class="span3">
            <p>
            </p>
        </div>
    </div>
    <div class="row">
        <button type="button" id="generate-report" class="btn btn-large btn-primary" disabled="disabled">
            Riport készítés
        </button>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            IsAuthenticated();

            weekNumberSetDefault();
        });

        $("#trello-boards").change(function () {
            var board = $(this).find(":selected").val();
            FillLists(board);
            FillUsers(board);
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
