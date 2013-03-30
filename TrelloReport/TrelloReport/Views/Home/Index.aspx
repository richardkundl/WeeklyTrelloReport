<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrelloReport.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trello Weekly Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <%= Model.Message %></h1>
    <p>
        <a href="" id="trello-authorize" target="_blank">Authorize me</a>
        <select id="trello-boards" class="span3">
            <option value="" selected="selected">Choose one</option>
        </select>
    </p>
    <p>
    </p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            IsAuthenticated();
        });
        
        $("#trello-boards").change(function(){
            FillLists($(this).find(":selected").val());
        });
    </script>
</asp:Content>
