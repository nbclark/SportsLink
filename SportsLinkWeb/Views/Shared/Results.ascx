<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var results = (ResultsModel)Model; %>
    <% var perPage = 10; %>
    <% var pageModel = PageModel.Create((int)ViewData["page"], perPage, results.UserResults.Count()); %>
    
    <div class="module" id="results" data-type="Results" style='display:<%=(results.UserResults.Count() > 0) ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Recent Results</h3>
            <div class="data">
                <table>
                <% foreach (OfferModel o in results.UserResults.Skip(pageModel.Skip).Take(perPage)) { %>
                    <% var isRequestor = (o.RequestUser.FacebookId == results.TennisUser.FacebookId); %>
                    <% var opponent = (!isRequestor) ? o.RequestUser : o.AcceptUser; %>
                    <tr class="result">
                        <td>
                            <img src="http://graph.facebook.com/<%=opponent.FacebookId %>/picture" />
                        </td>
                        <td class="opponent">
                            <a class="name" href="javascript:SportsLinkScript.Shared.Utility.showPlayerDetails('playerDetailsCard', '<%=opponent.Name %>', '<%=opponent.FacebookId %>');"><%=opponent.Name%></a>
                            <br />
                            <i><%=(isRequestor) ? o.RequestComments : o.AcceptComments %></i>
                        </td>
                        <td class="score">
                            <input type="hidden" class="offerId" value="<%=o.OfferId %>" />
                            <input type="hidden" class="score" value="<%=o.Score %>" />
                            <input type="hidden" class="requestName" value="<%=o.AcceptUser.Name %>" />
                            <input type="hidden" class="acceptName" value="<%=o.RequestUser.Name %>" />
                            <% if (string.IsNullOrEmpty(o.Score)) { %>
                                <a href="#" class="inputScore">Input Score</a>
                            <% } %>
                            <% else { %>
                                <a href="#" class="inputScore"><%=o.Score %></a>
                            <% } %>
                        </div>
                        <td class="time">
                            <%=IndexModel.FormatDate(o.MatchDateUtc, results.TennisUser.TimeZoneOffset).Replace(",", "<br />")%>
                        </div>
                    </tr>
                <% } %>
                </table>
                <% Html.RenderPartial("Paginator", pageModel); %>
            </div>
        </div>
    </div>

    <div id="scoredialog" title="Report Score" style="display:none;">
        <table width="100%">
            <tr>
                <th></th>
                <th class="requestName"></th>
                <th class="acceptName"></th>
            </tr>
            <% for (int i = 0; i < 5; ++i) { %>
                <tr id="score<%=i %>" class="score">
                    <td>Set <%=i+1 %></td>
                    <td><input type="text" id="request<%=i %>" /></td>
                    <td><input type="text" id="accept<%=i %>" /></td>
                </tr>
            <% } %>
                <tr>
                <td colspan="3" class="comments">Comments:<br /><input type="text" id="scoreComments" /></td>
                </tr>
        </table>
    </div>
