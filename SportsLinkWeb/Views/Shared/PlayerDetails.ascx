<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

<% var tennisUser = (TennisUserModel)ViewData["PlayerModel"]; %>

<link href="/Content/Site.css" rel="stylesheet" type="text/css" />

<table id="playerDetails">
    <tr>
        <td id="playerImage">
            <img src="http://graph.facebook.com/<%=tennisUser.FacebookId %>/picture" />
        </td>
        <td id="playerName">
            <%=tennisUser.Name %><br />
            <span id="playerLocation"><%=tennisUser.City.Name %></span>
        </td>
        <td id="playerRating"><%= IndexModel.FormatRating(tennisUser.Rating) %></td>
    </tr>
</table>