<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var o = (OfferModel)Model; %>
    <% var user = (TennisUserModel)ViewData["TennisUser"]; %>
    <% var isRequestor = (o.RequestUser.FacebookId == user.FacebookId); %>
    <% var opponent = (!isRequestor || null == o.ConfirmedUser) ? o.RequestUser : o.ConfirmedUser; %>
    <% var inFuture = o.MatchDateUtc > DateTime.UtcNow; %>

    <tr class="offer">
        <td class="image">
            <img src="http://graph.facebook.com/<%=opponent.FacebookId %>/picture" />
        </td>
        <td class="details">
            <a class="name" href="javascript:SportsLinkScript.Shared.Utility.showPlayerDetails('playerDetailsCard', '<%=opponent.Name %>', '<%=opponent.FacebookId %>');"><%=opponent.Name%></a>
            <div class="location"><%=o.GetLocationLink()%></div>
        </td>
        <td><div class="rating"><%=IndexModel.FormatRating(opponent.Rating)%></div><div>ntrp</div></td>
        <td><div class="age"><%=IndexModel.FormatAge(opponent.Birthday)%></div><div class="years">years</td>
        <td class="accept" align="right">
        
            <% if (null != o.ConfirmedUser) { %>
                <% if (inFuture)
                    { %>
                    <input type="hidden" class="offerId" value="<%=o.OfferId %>" />
                    <a href="#" class="cancelConfirmedMatch">Cancel</a>
                <% } else { %>
                    <input type="hidden" class="offerId" value="<%=o.OfferId %>" />
                    <input type="hidden" class="score" value="<%=o.Score %>" />
                    <a href="#" class="inputScore">Input Score</a>
                <% } %>
            <% } %>
            <% else { %>
                <div class="pending" style='display:<%=o.UserPending ? "" : "none" %>'>Pending Confirmation<br /><%=IndexModel.FormatDate(o.MatchDateUtc, user.TimeZoneOffset).Replace(",", " ")%></div>
                <input type="hidden" name="offerid" value="<%=o.OfferId %>" />
                <a style='display:<%=!o.UserPending ? "" : "none" %>' class="acceptMatch" href="#"><%=IndexModel.FormatDate(o.MatchDateUtc, user.TimeZoneOffset).Replace(",", "<br />")%></a>
            <% } %>
        </td>
    </tr>