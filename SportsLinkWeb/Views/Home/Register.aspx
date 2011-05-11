<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" Title="Register for TennisLink" %>
<%@ Import Namespace="Facebook.Web" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sign up for TennisLink</h2>
    <p>
        <fb:registration fields="[
{'name':'name'}, {'name':'email'}, {'name':'birthday'}, {'name':'gender'},
{'name':'location'},
{'name':'NTRPRating','description':'NTRP Rating', 'type':'select',
    'options' :
        {
            '1.5': '1.5', '1.75': '1.5-2.0', '2.0': '2.0', '2.25': '2.0-2.5', '2.5': '2.5', '2.75': '2.5-3.0',
            '3.0': '3.0', '3.25': '3.0-3.5', '3.5': '3.5', '3.75': '3.5-4.0',
            '4.0': '4.0', '4.25': '4.0-4.5', '4.5': '4.5', '4.75': '4.5-5.0', '5.0': '5.0+'
        }, 'default' : '4.0'
}, {'name':'SinglesDoubles','description':'Play Preference', 'type':'select',
    'options' :
        {
            'Singles':'Singles', 'Doubles':'Doubles', 'Either':'Either'
        }, 'default' : 'Either'
}
            ]"
            redirect-uri="<%=CanvasSettings.Current.CanvasPageUrl %>"
            perms="user_about_me,user_birthday,user_location"
            >
        </fb:registration>
    </p>
</asp:Content>
