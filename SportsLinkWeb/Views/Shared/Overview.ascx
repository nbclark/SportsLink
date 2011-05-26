<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="SportsLink" %>
<% var userModel = (IndexModel)Model; %>
<div class="module">
    <div class="ui-widget-content ui-corner-all">
        <h3 class="ui-widget-header ui-corner-all">
            Welcome to TennisLink
        </h3>
        <div class="data">
            <p>
                Welcome to TennisLink. We want to make it easy for you to find competitive matches
                in your area.
            </p>
            <p>
                Getting started is really easy. We know your rating and your location. We will let
                you know when other players at your level are looking for a match.
            </p>
            <p>
                You can also schedule a match for a certain date and location, and we will reach
                out to other players in your area to find a match. As soon as we find a match, we
                will send you an email.
            </p>
            <p>
                Enjoy!
            </p>
        </div>
    </div>
</div>
