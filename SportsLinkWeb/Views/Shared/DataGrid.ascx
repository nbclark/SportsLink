<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

<% var gridModel = (DataGridModel)Model; %>
<% var perPage = 30; %>
<% var pageModel = PageModel.Create((int)ViewData["page"], perPage, gridModel.Rows.Count()); %>

<div class="grid">
    <table>
        <% if (gridModel.ShowHeader) { %>
            <tr>
                <% foreach (var column in gridModel.Columns) { %>
                    <th><%=column.Title %></th>
                <% } %>
            </tr>
        <% } %>
        <% foreach (var row in gridModel.Rows) { %>
            <tr>
                <% foreach (var column in gridModel.Columns) { %>
                    <td><%=gridModel.RenderCell(row, column) %></td>
                <% } %>
            </tr>
        <% } %>
    </table>
    <% Html.RenderPartial("Paginator", pageModel); %>
</div>