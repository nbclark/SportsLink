<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var playerModel = (PlayerModel)ViewData["PlayerModel"]; %>

    <div class="module" id="players" data-type="Players" style='display:<%=(playerModel.Players.Count > 0) ? "" : "none" %>'>
        <input type="hidden" id="playersPage" value="<%=playerModel.Page %>" />
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Similar Players</h3>
            <div class="data">
                <table width="100%">
                <%foreach (TennisUserModel user in playerModel.Players) { %>
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
                <% if (playerModel.HasPrev) { %>
                    <a id="playersPrev" href="#">Prev</a>
                <% } %>
                <% if (playerModel.HasNext) { %>
                    <a id="playersNext" href="#">Next</a>
                <% } %>
            </div>
        </div>
    </div>

    <div id="challengeDialog" class="module quickmatch" data-type="QuickMatch" style="display:none">
        <% Html.RenderPartial("CreateMatch"); %>
    </div>