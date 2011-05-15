<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

<% var model = (ModuleModel)Model; %>

<div class="data form">
    <div class="label">When:</div>
    <div class="input">
        <% var userTime = DateTime.UtcNow.AddHours(model.TennisUser.TimeZoneOffset); %>
        <% userTime = (userTime.Hour >= 18) ? userTime.AddDays(1) : userTime; %>

        <input type="text" class="datepicker" value="<%=userTime.ToString("MM/dd/yyyy") %>">
        <select class="time">
            <% for (DateTime i = DateTime.Now.Date.AddHours(13); i <= DateTime.Now.Date.AddDays(1); i = i.AddMinutes(15)) { %>
                <option <%=(i.Hour == 19 && i.Minute == 0) ? "selected=selected" : "" %>><%=i.ToString("hh:mm")%></option>
            <% } %>
        </select>
        <select class="ampm">
            <option>AM</option>
            <option selected="selected">PM</option>
        </select>
    </div>
    <div class="label">Location (optional):</div>
    <div class="input full">
        <input type="hidden" class="placesAutoValue" />
        <input class="placesAutoFill" data-location='<%= string.Concat(model.TennisUser.City.Latitude, ",", model.TennisUser.City.Longitude) %>' data-accesstoken='<%=ConfigurationManager.AppSettings["GoogleAccessToken"] %>' />
    </div>
    <div class="label">Comments (optional):</div>
    <div class="input full">
        <input class="comments" type="text" />
    </div>
</div>