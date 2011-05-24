<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var playerModel = (PlayersModel)Model; %>
    <% var perPage = 3; %>
    <% var pageModel = PageModel.Create((int)ViewData["page"], perPage, playerModel.Players.Count()); %>

    <div class="module" id="players" data-type="Players" style='display:<%=(playerModel.Players.Count() > 0) ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">
                Similar Players
                <a href="#" class="more" style="display: inline; float:right; white-space:nowrap" title="View More"><span style="float:left">All</span> <span class="ui-icon ui-icon-person" style="display: inline; float:right;"></span></a>
            </h3>
            <div class="data">
                <table width="100%" cellpadding="0" cellspacing="0">
                <%foreach (TennisUserModel user in playerModel.Players.Skip(pageModel.Skip).Take(perPage)) { %>
                    <tr>
                        <td class="image">
                            <img src="http://graph.facebook.com/<%=user.FacebookId %>/picture" />
                        </td>
                        <td class="title">
                            <a class="name" href="javascript:SportsLinkScript.Shared.Utility.showPlayerDetails('playerDetailsCard', '<%=user.Name %>', '<%=user.FacebookId %>');"><%=user.Name %></a>
                            <div class="location"><%=user.City.Name %></div>
                        </td>
                        <td><div class="rating"><%=IndexModel.FormatRating(user.Rating) %></div><div>ntrp</div></td>
                        <td><div class="age"><%=IndexModel.FormatAge(user.Birthday) %></div><div class="years">years</td>
                        <td class="challenge">
                            <a href="#" class="requestMatch" title="Challenge <%=user.Name %>" id="<%=user.FacebookId %>">Play</a>
                        </td>
                    </tr>
                <% } %>
                </table>
                <!--
                <% Html.RenderPartial("Paginator", pageModel); %>
                -->
            </div>
        </div>
    </div>

    <div id="challengeDialog" class="module quickmatch" data-type="QuickMatch" style="display:none">
        <% Html.RenderPartial("CreateMatch", Model); %>
    </div>