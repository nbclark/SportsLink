<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewData["Message"] %></h2>
        <fb:login-button registration-url="http://localhost:57157/home/register" show-faces="false" perms="user_birthday,user_location,publish_stream,email"></fb:login-button>
</asp:Content>
