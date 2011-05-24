<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userOffersModel = (UserOffersModel)Model; %>
    <% var perPage = 5; %>
    <% var pageModel = PageModel.Create((int)ViewData["page"], perPage, userOffersModel.UserOffers.Count()); %>

    <div id="userChallenges" class="module" data-type="UserChallenges" style='display:<%=userOffersModel.UserOffers.Count() > 0 ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Your Match Requests</h3>
            <div class="data">
                <table>
                <% foreach (var request in userOffersModel.UserOffers.Skip(pageModel.Skip).Take(perPage)) { %>
                    <tr>
                        <td class="cancel">
                            <a class="cancelMatch" href="#" data-offerId='<%=request.OfferId %>'>Cancel</a>
                        </td>
                        <td class="time">
                            <%=IndexModel.FormatDate(request.MatchDateUtc, userOffersModel.TennisUser.TimeZoneOffset).Replace(",", "<br />")%>
                        </td>
                        <td class="location">
                            <%=request.GetLocationLink()%>
                        </td>
                        <td class="confirm">
                            <a class="confirmOffers" href="#" data-offerId='<%=request.OfferId %>'><%=(request.AcceptedUsers != null) ? request.AcceptedUsers.Count() : 0 %> Offers</a>
                        </td>
                    </tr>
                <% } %>
                </table>
            </div>
        </div>
    
    </div>