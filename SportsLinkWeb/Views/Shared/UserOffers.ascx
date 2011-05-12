<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userModel = (UserModel)ViewData["UserModel"]; %>
    <% var indexModel = (IndexModel)ViewData["IndexModel"]; %>

    <div id="userOffers" class="module" data-type="UserOffers" style='display:<%=(indexModel.UserOffers.Count > 0) ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Your Match Requests
            </h3>
            <div class="data">
                <table>
                <% foreach (var request in indexModel.UserOffers) { %>
                    <tr>
                        <td class="time">
                            <%=IndexModel.FormatDate(request.MatchDateUtc, userModel.User.TimeZoneOffset).Replace(",", "<br />")%>
                        </td>
                        <td class="location">
                            <%=request.City.Name%>
                        </td>
                        <td class="accept">
                            <a class="cancelMatch" href="#" data-offerid='<%=request.OfferId %>'>Cancel</a>
                        </td>
                    </tr>
                <% } %>
                </table>
            </div>
        </div>
    </div>