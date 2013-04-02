<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrelloReport.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trello Weekly Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <%= Model.Message %></h1>
    <div class="row">
        <a href="" id="trello-authorize" target="_blank">Authorize me</a>
    </div>
    <div class="row">
        <div class="span3">
            <select id="trello-boards" class="span3">
                <option value="" selected="selected">Choose one</option>
            </select>
        </div>
    </div>
    <div class="row">
        <div class="span3">
            <h6>Lista:</h6>
            <label class="checkbox"><input type="checkbox" checked="checked" id="board-lists-selectall"/>Összes</label>
            <div id="board-lists">
            </div>
        </div>
        <div class="span3">
            <h6>Felhasználó:</h6>
            <label class="checkbox"><input type="checkbox" checked="checked" id="board-users-selectall"/>Összes</label>
            <div id="board-users">
            </div>
        </div>
        <div class="span6">
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
        });

        $("#trello-boards").change(function () {
            var board = $(this).find(":selected").val();
            FillLists(board);
            FillUsers(board);
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
