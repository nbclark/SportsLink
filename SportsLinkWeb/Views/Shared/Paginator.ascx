<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>
<% var pageModel = (IPageModel)Model; %>
<input type="hidden" class="page" value="<%=pageModel.Page %>" />
<div style="text-align: right">
    <% if (pageModel.HasPrev)
       { %>
    <a class="prev" href="#">Prev</a>
    <% } %>
    <% if (pageModel.HasNext)
       { %>
    <a class="next" href="#">Next</a>
    <% } %>
</div>
