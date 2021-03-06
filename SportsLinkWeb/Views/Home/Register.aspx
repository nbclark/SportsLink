﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" Title="Register for TennisLoop" %>
<%@ Import Namespace="Facebook.Web" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sign up for TennisLoop</h2>
    <p style="max-height:500px;">
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
            fb_only="true"
            redirect-uri="<%=Facebook.FacebookApplication.Current.CanvasUrl %>"
            perms="user_birthday,user_location,publish_stream,email"
            >
        </fb:registration>
    </p>
</asp:Content>
