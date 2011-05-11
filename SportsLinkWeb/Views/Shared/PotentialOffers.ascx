<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userModel = (UserModel)ViewData["UserModel"]; %>
    <% var indexModel = (IndexModel)ViewData["IndexModel"]; %>

    <div class="module" style="display:none" data-type="PotentialOffers" display='<%=indexModel.PotentialOffers.Count > 0 ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Potential Matches
            </h3>
            <div class="data">
                <table>
                <% foreach (OfferModel o in indexModel.PotentialOffers) { %>
                <tr class="offer">
                    <td class="image">
                        <img src="http://graph.facebook.com/<%=o.RequestUser.FacebookId %>/picture" />
                    </td>
                    <td class="details">
                        <div class="name"><%=o.RequestUser.Name %></div>
                        <div class="rating"><%=IndexModel.FormatRating(o.RequestUser.Rating) %></div>
                        <div class="location"><%=o.City.Name %> (<%=o.City.GetDistanceInMiles(userModel.User.City) %> miles)</div>
                    </td>
                    <td class="time">
                        <%=IndexModel.FormatDate(o.MatchDateUtc, userModel.User.TimeZoneOffset).Replace(",", "<br />")%>
                    </td>
                    <td class="accept">
                        <input type="hidden" name="offerid" value="<%=o.OfferId %>" />
                        <a class="acceptMatch" href="#">Accept</a>
                        <a class="rejectMatch" href="#" style="display:none">Reject</a>
                    </td>
                </tr>
                <% } %>
                </table>
            </div>
        </div>
    </div>