<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta http-equiv="refresh" content="5;url=/">
    <h2>
        <%=Model %>
    </h2>
</asp:Content>
