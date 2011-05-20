<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var confirmedMatches = (ConfirmedMatchesModel)Model; %>
    <% var perPage = 3; %>
    <% var pageModel = PageModel.Create((int)ViewData["page"], perPage, confirmedMatches.ConfirmedMatches.Count()); %>
    
    <div class="module" id="confirmedMatches" data-type="ConfirmedMatches" style='display:block'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Confirmed Matches</h3>
            <div class="data">
                <table width="100%" cellpadding="0" cellspacing="0">
                <% foreach (OfferModel o in confirmedMatches.ConfirmedMatches.Skip(pageModel.Skip).Take(perPage)) { %>
                    <% var isRequestor = (o.RequestUser.FacebookId == confirmedMatches.TennisUser.FacebookId); %>
                    <% var opponent = (!isRequestor) ? o.RequestUser : o.AcceptUser; %>
                    <% var inFuture = o.MatchDateUtc > DateTime.UtcNow; %>

                    <tr class="confirmedMatch">
                        <td class="image">
                            <img src="http://graph.facebook.com/<%=opponent.FacebookId %>/picture" />
                        </td>
                        <td class="opponent">
                            <a class="name" href="javascript:SportsLinkScript.Shared.Utility.showPlayerDetails('playerDetailsCard', '<%=opponent.Name %>', '<%=opponent.FacebookId %>');"><%=opponent.Name%></a>
                            <div class="location"><%=o.GetLocationLink()%></div>
                            <br />
                            <i><%=(isRequestor) ? o.RequestComments : o.AcceptComments %></i>
                        </td>
                        <td class="cancelOrScore">
                            <input type="hidden" class="offerId" value="<%=o.OfferId %>" />
                            <input type="hidden" class="score" value="<%=o.Score %>" />
                            <% if (inFuture) { %>
                                <a href="#" class="cancelMatch">Cancel</a>
                            <% } %>
                            <% else if (string.IsNullOrEmpty(o.Score)) { %>
                                <a href="#" class="inputScore">Input Score</a>
                            <% } %>
                        </div>
                        <td class="time">
                            <% var time = IndexModel.FormatDate(o.MatchDateUtc, confirmedMatches.TennisUser.TimeZoneOffset).Replace(",", "<br />"); %>
                            <b><%=time %></b>
                        </div>
                    </tr>
                <% } %>
                </table>
                <% Html.RenderPartial("Paginator", pageModel); %>
            </div>
        </div>
    </div>