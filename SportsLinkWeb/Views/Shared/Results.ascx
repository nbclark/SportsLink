<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var results = (ResultsModel)Model; %>
    <% var perPage = 10; %>
    <% var pageModel = PageModel<OfferModel>.Create((int)ViewData["page"], perPage, results.UserResults); %>
    
    <div class="module" id="results" data-type="Results" style='display:<%=(pageModel.Items.Count() > 0) ? "" : "none" %>'>
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Previous Results</h3>
            <div class="data">
                <table width="100%" cellpadding="0" cellspacing="0">
                <% foreach (OfferModel o in pageModel.Items) { %>
                    <% var isRequestor = (o.RequestUser.FacebookId == results.TennisUser.FacebookId); %>
                    <% var opponent = (!isRequestor) ? o.RequestUser : o.ConfirmedUser; %>
                    <% var inFuture = o.MatchDateUtc > DateTime.UtcNow; %>

                    <tr class="result">
                        <td class="image">
                            <img src="http://graph.facebook.com/<%=opponent.FacebookId %>/picture" />
                        </td>
                        <td class="opponent">
                            <a class="name" href="javascript:SportsLinkScript.Shared.Utility.showPlayerDetails('playerDetailsCard', '<%=opponent.Name %>', '<%=opponent.FacebookId %>');"><%=opponent.Name%></a>
                            <div class="location"><%=o.GetLocationLink()%></div>
                            <br />
                            <i><%=(isRequestor) ? o.RequestComments : o.AcceptComments %></i>
                        </td>
                        <td class="score">
                            <input type="hidden" class="offerId" value="<%=o.OfferId %>" />
                            <input type="hidden" class="score" value="<%=o.Score %>" />
                            <input type="hidden" class="requestName" value="<%=o.ConfirmedUser.Name %>" />
                            <input type="hidden" class="acceptName" value="<%=o.RequestUser.Name %>" />

                            <% if (inFuture) { %>
                                Cancel
                            <% } %>
                            <% else if (string.IsNullOrEmpty(o.Score)) { %>
                                <a href="#" class="inputScore">Input Score</a>
                            <% } %>
                            <% else { %>
                                <a href="#" class="inputScore"><%=o.Score %></a>
                            <% } %>
                        </div>
                        <td class="time">
                            <% var time = IndexModel.FormatDate(o.MatchDateUtc, results.TennisUser.TimeZoneOffset).Replace(",", "<br />"); %>
                            <% if (inFuture) { %>
                                <b><%=time %></b>
                            <% } %>
                            <% else { %>
                                <%=time %>
                            <% } %>
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
