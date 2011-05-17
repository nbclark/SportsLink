<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% var tennisUser = (TennisUserModel)Model; %>

<a href="#" class="requestMatch" title="Challenge <%=tennisUser.Name %>" id="<%=tennisUser.FacebookId %>">Play</a>
