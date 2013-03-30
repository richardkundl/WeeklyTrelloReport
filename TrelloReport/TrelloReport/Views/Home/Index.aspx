<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrelloReport.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            var authUrl = "<%= Url.Action("IsAuthenticated","Api") %>";
            IsAuthenticated(authUrl);
        });
    </script>
    <h2>
        <%= Model.Message %></h2>
    <a href="" id="trello-authorize" target="_blank">Authorize me</a>
    <select id="trello-boadrs">
        <option value="" selected="selected">Choose one</option>
    </select>
    <p>
        To learn more about ASP.NET MVC visit <a href="http://asp.net/mvc" title="ASP.NET MVC Website">
            http://asp.net/mvc</a>.
    </p>
</asp:Content>
