<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <% var userModel = (UserModel)ViewData["UserModel"]; %>

    <div class="module quickmatch" data-type="QuickMatch">
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Find a Match</h3>
            <% Html.RenderPartial("CreateMatch"); %>
            <div class="submit">
                <a class="findMatch" href="#">Find a Match</a>
            </div>
        </div>
    </div>