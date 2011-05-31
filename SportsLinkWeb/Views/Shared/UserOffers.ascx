<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userOffersModel = (UserOffersModel)Model; %>
    <% var perPage = 5; %>
    <% var pageModel = PageModel<OfferModel>.Create((int)ViewData["page"], perPage, userOffersModel.UserOffers); %>

    <div id="userOffers" class="module" data-type="UserOffers" style='display:<%=(pageModel.Items.Count() > 0) ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Your Match Requests
            </h3>
            <div class="data">
                <table>
                <% foreach (var request in pageModel.Items) { %>
                    <tr>
                        <td class="time">
                            <%=IndexModel.FormatDate(request.MatchDateUtc, userOffersModel.TennisUser.TimeZoneOffset).Replace(",", "<br />")%>
                        </td>
                        <td class="location">
                            <%=request.GetLocationLink()%>
                        </td>
                        <td class="accept">
                            <a class="cancelMatch" href="#" data-offerId='<%=request.OfferId %>'>Cancel</a>
                        </td>
                    </tr>
                <% } %>
                </table>
                <% Html.RenderPartial("Paginator", pageModel); %>
            </div>
        </div>
    </div>