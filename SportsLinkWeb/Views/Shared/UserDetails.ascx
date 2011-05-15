<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userModel = (ModuleModel)Model; %>

    <div class="module">
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Your Details</h3>
            <div class="data">
                <div class="keyvaluerow">
                    <div class="name">Name</div>
                    <div class="value"><%=userModel.TennisUser.Name%></div>
                </div>
                <div class="keyvaluerow">
                    <div class="name">Location</div>
                    <div class="value"><%=userModel.TennisUser.City.Name%></div>
                </div>
                <div class="keyvaluerow">
                    <div class="name">Rating</div>
                    <div class="value"><%=IndexModel.FormatRating(userModel.TennisUser.Rating)%></div>
                </div>
            </div>
        </div>
    </div>