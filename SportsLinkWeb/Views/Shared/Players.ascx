<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var playerModel = (PlayersModel)Model; %>
    <% var perPage = 5; %>
    <% var pageModel = PageModel.Create((int)ViewData["page"], perPage, playerModel.Players.Count()); %>

    <div class="module" id="players" data-type="Players" style='display:<%=(playerModel.Players.Count() > 0) ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Similar Players</h3>
            <div class="data">
                <table width="100%">
                <%foreach (TennisUserModel user in playerModel.Players.Skip(pageModel.Skip).Take(perPage)) { %>
                    <tr>
                        <td>
                            <img src="http://graph.facebook.com/<%=user.FacebookId %>/picture" />
                        </td>
                        <td>
                            <a class="name" href="javascript:SportsLinkScript.Shared.Utility.showPlayerDetails('playerDetailsCard', '<%=user.Name %>', '<%=user.FacebookId %>');"><%=user.Name %></a>
                            <div class="location"><%=user.City.Name %></div>
                        </td>
                        <td class="rating"><%=IndexModel.FormatRating(user.Rating) %></td>
                        <td class="challenge">
                            <a href="#" class="requestMatch" title="Challenge <%=user.Name %>" id="<%=user.FacebookId %>">Challenge</a>
                        </td>
                    </tr>
                <% } %>
                </table>
                <% Html.RenderPartial("Paginator", pageModel); %>
            </div>
        </div>
    </div>

    <div id="challengeDialog" class="module quickmatch" data-type="QuickMatch" style="display:none">
        <% Html.RenderPartial("CreateMatch", Model); %>
    </div>