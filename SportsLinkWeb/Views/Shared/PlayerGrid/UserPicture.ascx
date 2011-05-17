<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% var tennisUser = (TennisUserModel)Model; %>

<img src='http://graph.facebook.com/<%=tennisUser.FacebookId %>/picture' />