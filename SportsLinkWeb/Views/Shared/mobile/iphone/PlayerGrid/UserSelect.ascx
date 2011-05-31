<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% var tennisUser = (TennisUserModel)Model; %>

<a href="#" class="selectUser" title="Select <%=tennisUser.Name %>" data-fbId="<%=tennisUser.FacebookId %>">Select</a>

