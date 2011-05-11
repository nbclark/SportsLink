<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userModel = (UserModel)ViewData["UserModel"]; %>

    <div class="module">
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Welcome <%=userModel.User.Name %>
            </h3>
            <div class="data">
                Welcome to TennisLink.  We want to make it easy for you to find competitive
                matches in your area.
            </div>
        </div>
    </div>