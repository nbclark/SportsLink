<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewData["Message"] %></h2>
    <p id="login">
        <fb:login-button registration-url="http://sportslink.cloudapp.net/home/register" show-faces="true" perms="user_about_me,user_birthday,user_location" />
    </p>
</asp:Content>
