<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

<% var tennisUser = (TennisUserModel)ViewData["PlayerModel"]; %>

<table id="playerDetails" data-type="PlayerDetails">
    <link href="/Content/Site.css" rel="stylesheet" type="text/css" />
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
    <tr>
        <td id="playerMessage" colspan="3">
            <input class="comments" type="text" /> <a class="sendMessage" href="#">Send Message</a>
        </td>
    </tr>
</table>