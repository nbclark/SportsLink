using System;
using System.Collections;
using System.Html;
using System.Net;
using System.Serialization;
using System.Runtime.CompilerServices;
using jQueryApi;
using SportsLinkScript.Shared;
using SportsLinkScript.Shared.Facebook;
using SportsLinkScript.Shared.Html;

namespace SportsLinkScript.Pages
{
    public static class Login
    {
        public static string AccessToken;

        public static void Init(string appId, string action)
        {
            bool isLoginPage = action == "Login" || action == "Register";

            FbWindow.FbAsyncInit = delegate()
            {
                FB.Init(new JsonObject("appId", appId, "cookie", true, "status", true, "xfbml", true));
                /*
                FB.GetLoginStatus((FBSubscribeHandler)delegate(object r)
                {
                    AuthResponse response = (AuthResponse)r;
                    return;
                    if (response.Status == "connected")
                    {
                        if (action == "Login")
                        {
                            jQuery.Select("#login").Hide();
                            Window.Location.Href = "/home/index";
                        }
                        else
                        {
                            jQuery.Select("#main").Show(EffectDuration.Slow);
                        }
                    }
                    else if (response.Status == "notConnected" && action != "Register")
                    {
                        jQuery.Select("#login").Hide();
                        Window.Location.Href = "/home/register";
                    }
                });
                */
                FB.Event.Subscribe("auth.statusChange", (FBSubscribeHandler)delegate(object r)
                {
                });

                FB.Event.Subscribe("auth.login", (FBSubscribeHandler)delegate(object r)
                {
                    AuthResponse response = (AuthResponse)r;

                    return;

                    if (response.Status == "connected")
                    {
                        jQuery.Select("#login").Hide();
                        //Window.Location.Href = "/index";
                        return;
                    }

                    if (isLoginPage)
                    {
                        AccessToken = response.Session.Access_token;
                        Query query = FB.Data.Query("select first_name, last_name, birthday, uid from user where uid={0}", response.Session.Uid);

                        query.Wait(ProcessLogin);
                    }
                    else
                    {
                        jQuery.Select("#main").Show(EffectDuration.Slow);
                    }
                });

                return null;
            };

            H5ScriptElement e = (H5ScriptElement)Document.CreateElement("script");
            e.Async = true;
            e.Src = "http://connect.facebook.net/en_US/all.js";
            Document.GetElementById("fb-root").AppendChild(e);
        }

        public static void ProcessLogin(Array rows)
        {
            LoginRow row = (LoginRow)rows[0];

            jQuery.Post("/Services/AddUser?signed_request=" + Utility.GetSignedRequest(), new JsonObject("member", new JsonObject()), (AjaxRequestCallback)delegate(object data, string textStatus, XmlHttpRequest request)
                {
                    WebServiceResponse response = (WebServiceResponse)data;
                    AddUserResponse addUser = (AddUserResponse)response.Data;

                    //
                }
            );
        }
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    internal class LoginRow
    {
        public long uid;
        public string first_name;
        public string last_name;
    }
}
