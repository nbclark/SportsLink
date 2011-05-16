<%@  Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>
<% var model = (CalendarModel)ViewData.Model; %>
<% ViewData["TennisUser"] = model.TennisUser; %>
<% var pageModel = WeekPageModel.Create((int)ViewData["page"]); %>

<div class="module" data-type="Calendar">
<table class="calendar" cellpadding="0" cellspacing="0">
    <% for (int i = 0; i < 7; ++i) { %>
        <tr>
            <th colspan="5">
                <%= model.UserStartDate.AddDays(i).ToShortDateString() %>
            </th>
        </tr>
        <% var dayOffers = model.GetOffers(i); %>

        <% if (dayOffers.Count() == 0) { %>
        <tr>
            <td class="empty" colspan="5">-- Nothing Scheduled --</td>
        </tr>
        <% } %>

        <% foreach (OfferModel o in dayOffers) { %>
        <% Html.RenderPartial("Offer", o); %>
        <% } %>
    <% } %>
</table>
<div class="calendarpage">
<% Html.RenderPartial("Paginator", pageModel); %>
</div>
</div>