<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>

    <div class="module quickmatch" data-type="QuickMatch">
        <div class="ui-widget-content ui-corner-all">
            <h3 class="ui-widget-header ui-corner-all">Find a Match</h3>
            <% Html.RenderPartial("CreateMatch", Model); %>
            <div class="submit">
                <a class="findMatch" href="#">Find a Match</a>
            </div>
        </div>
    </div>