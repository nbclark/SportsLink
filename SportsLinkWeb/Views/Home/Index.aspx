<%@ Page Language="C#" Title="Tennis Link" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="SportsLink" %>
<%@ Import Namespace="System.Configuration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% var indexModel = (IndexModel)ViewData.Model; %>

    <div id="playerDetailsCard" style="display:none">
        <div id="module_PlayerDetails">
        </div>
    </div>

    <div id="calendarCard" style="display:none">
        <div id="module_Calendar">
        </div>
    </div>

    <table id="content">
    <tr>
    <td class="left">
    
    <% Html.RenderPartial("Overview", indexModel); %>
    
    <div id="module_PotentialOffers">
    <% Html.RenderPartial("PotentialOffers", indexModel.GetModel<PotentialOffersModel>()); %>
    </div>

    <div id="module_Results">
    <% Html.RenderPartial("Results", indexModel.GetModel<ResultsModel>()); %>
    </div>

    <div id="module_Players">
    <% Html.RenderPartial("Players", indexModel.GetModel<PlayersModel>()); %>
    </div>
    

    </td>
    <td class="right">
    
    
    <div id="module_QuickMatch">
    <% Html.RenderPartial("QuickMatch", indexModel); %>
    </div>
    <div id="module_UserOffers">
    <% Html.RenderPartial("UserOffers", indexModel.GetModel<UserOffersModel>()); %>
    </div>
    <div id="module_UserDetails">
    <% Html.RenderPartial("UserDetails", indexModel); %>
    </div>

    <div class="module">
    <fb:login-button show-faces="true" width="220"></fb:login-button>
    </div>
    </td>
    </tr>
    </table>
</asp:Content>
