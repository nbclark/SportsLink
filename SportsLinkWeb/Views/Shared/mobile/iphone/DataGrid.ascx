<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <link href="/Content/Site.css" rel="stylesheet" type="text/css" />
<% var gridModel = (DataGridModel)Model; %>
<% var perPage = 20; %>
<% var pageModel = PageModel.Create((int)ViewData["page"], perPage, gridModel.Rows.Count()); %>

<div class="grid">
    <table>
        <% if (gridModel.ShowHeader) { %>
            <tr>
                <% foreach (var column in gridModel.Columns) { %>
                    <th>
                        <% if (column.CanFilter) { %>
                            <select name='<%=column.Name %>' multiple="multiple" title='<%=column.Title %>'>
                                <% foreach (var val in gridModel.GetDistinctValues(column)) { %>
                                    <option value='<%=val.Value %>' <%= (val.Selected ? "selected" : "") %>><%=val.Name %></option>
                                <% } %>
                            </select>
                        <% } %>
                        <% else { %>
                        <%=column.Title %>
                        <% } %>
                    </th>
                <% } %>
            </tr>
        <% } %>
        <% foreach (var row in gridModel.Rows.Skip(pageModel.Skip).Take(pageModel.ItemsPerPage)) { %>
            <tr>
                <% foreach (var column in gridModel.Columns) { %>
                    <td>
                        <% if (!string.IsNullOrEmpty(column.View)) { %>
                            <% Html.RenderPartial(column.View, row); %>
                        <% } %>
                        <% else { %>
                            <%=gridModel.GetCellValue(row, column) %>
                        <% } %>
                    </td>
                <% } %>
            </tr>
        <% } %>
    </table>
    <% Html.RenderPartial("Paginator", pageModel); %>
</div>