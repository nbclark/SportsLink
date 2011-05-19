<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var o = (OfferModel)Model; %>
    <% var user = (TennisUserModel)ViewData["TennisUser"]; %>

    <tr class="offer">
        <td class="image">
            <img src="http://graph.facebook.com/<%=o.RequestUser.FacebookId %>/picture" />
        </td>
        <td class="details">
            <a class="name" href="javascript:SportsLinkScript.Shared.Utility.showPlayerDetails('playerDetailsCard', '<%=o.RequestUser.Name %>', '<%=o.RequestUser.FacebookId %>');"><%=o.RequestUser.Name %></a>
            <div class="location"><%=o.GetLocationLink()%></div>
        </td>
        <td><div class="rating"><%=IndexModel.FormatRating(user.Rating)%></div><div>ntrp</div></td>
        <td><div class="age"><%=IndexModel.FormatAge(user.Birthday)%></div><div class="years">years</td>
        <td class="accept" align="right">
            <% if (o.UserPending) { %>
                <div class="pending">pending<br /><%=IndexModel.FormatDate(o.MatchDateUtc, user.TimeZoneOffset).Replace(",", " ")%></div>
            <% } %>
            <% else { %>
            <input type="hidden" name="offerid" value="<%=o.OfferId %>" />
            <a class="acceptMatch" href="#"><%=IndexModel.FormatDate(o.MatchDateUtc, user.TimeZoneOffset).Replace(",", "<br />")%></a>
            <a class="rejectMatch" href="#" style="display:none">Reject</a>
            <% } %>
        </td>
    </tr>