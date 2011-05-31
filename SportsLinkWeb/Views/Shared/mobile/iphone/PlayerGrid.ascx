<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="module" data-type="PlayerGrid">
<% Html.RenderPartial("DataGrid", this.Model); %>
</div>