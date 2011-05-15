<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var potentialOffers = (PotentialOffersModel)Model; %>
    <% var perPage = 5; %>
    <% var pageModel = PageModel.Create((int)ViewData["page"], perPage, potentialOffers.PotentialOffers.Count()); %>

    <div class="module" data-type="PotentialOffers" style='display:<%=potentialOffers.PotentialOffers.Count() > 0 ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Match Requests</h3>
            <div class="data">
                <table>
                <% foreach (OfferModel o in potentialOffers.PotentialOffers.Skip(pageModel.Skip).Take(perPage)) { %>
                <tr class="offer">
                    <td class="image">
                        <img src="http://graph.facebook.com/<%=o.RequestUser.FacebookId %>/picture" />
                    </td>
                    <td class="details">
                        <div class="name"><%=o.RequestUser.Name %></div>
                        <div class="rating"><%=IndexModel.FormatRating(o.RequestUser.Rating) %></div>
                        <div class="location"><%=o.City.Name %> (<%=o.City.GetDistanceInMiles(potentialOffers.TennisUser.City)%> miles)</div>
                    </td>
                    <td class="time">
                        <%=IndexModel.FormatDate(o.MatchDateUtc, potentialOffers.TennisUser.TimeZoneOffset).Replace(",", "<br />")%>
                    </td>
                    <td class="accept">
                        <input type="hidden" name="offerid" value="<%=o.OfferId %>" />
                        <a class="acceptMatch" href="#">Accept</a>
                        <a class="rejectMatch" href="#" style="display:none">Reject</a>
                    </td>
                </tr>
                <% } %>
                </table>
                <% Html.RenderPartial("Paginator", pageModel); %>
            </div>
        </div>
    </div>