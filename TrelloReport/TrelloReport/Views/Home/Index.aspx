<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrelloReport.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%= Model.Message %></h2>
    <% if (string.IsNullOrEmpty(Model.TrelloUserKey))
       { %>
    <a href="<%= Model.TrelloAuthUrl %>" target="_blank">Authorize me</a>
    <% }
       else
       { %>
    <select>
        <% foreach (var board in Model.Boards)
           { %>
        <option value="<%= board.Id %>">
            <%= board.Name %></option>
        <%   } %>
    </select>
    <% } %>
    <p>
        To learn more about ASP.NET MVC visit <a href="http://asp.net/mvc" title="ASP.NET MVC Website">
            http://asp.net/mvc</a>.
    </p>
</asp:Content>
