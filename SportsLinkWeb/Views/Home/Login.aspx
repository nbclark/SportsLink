<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewData["Message"] %></h2>
        <fb:login-button registration-url="http://localhost/home/register" show-faces="false" perms="user_birthday,user_location,publish_stream,email">Login with Facebook</fb:login-button>

        <a href='https://www.facebook.com/dialog/oauth?client_id=<%= Facebook.FacebookApplication.Current.AppId%>&redirect_uri=<%= HttpUtility.UrlEncode(Facebook.FacebookApplication.Current.CanvasPage) %>'>Old Login</a>
</asp:Content>
