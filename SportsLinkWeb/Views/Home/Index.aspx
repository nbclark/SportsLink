<%@ Page Language="C#" Title="Tennis Link" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="SportsLink" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% var userModel = (UserModel)ViewData["UserModel"]; %>

    <div id="playerDetailsCard" style="display:none">
        <div id="module_PlayerDetails">
        </div>
    </div>

    <table id="content">
    <tr>
    <td class="left">
    
    <% Html.RenderPartial("Overview"); %>
    <% Html.RenderPartial("PotentialOffers"); %>

    <div id="module_Results">
    <% Html.RenderPartial("Results"); %>
    </div>

    <div id="module_Players">
    <% Html.RenderPartial("Players"); %>
    </div>
    

    </td>
    <td class="right">
    
    <% Html.RenderPartial("QuickMatch"); %>
    <div id="module_UserOffers">
    <% Html.RenderPartial("UserOffers"); %>
    </div>
    <% Html.RenderPartial("UserDetails"); %>

    <div class="module">
    <fb:login-button show-faces="true" width="240"></fb:login-button>
    </div>
    </td>
    </tr>
    </table>
</asp:Content>
