// HomePage.cs
//

using System;
using System.Collections;
using System.Html;
using System.Net;
using jQueryApi;
using SportsLinkScript.Shared;
using System.Serialization;

namespace SportsLinkScript.Controls
{
    public class PlayerGrid : PaginatedModule
    {
        public PlayerGrid(Element element)
            : base(element, "PlayerGrid")
        {
            // Wire up the select user even for confirming a user
            jQueryUIObject selectUser = (jQueryUIObject)this.Obj.Find(".selectUser");
            selectUser.Button(new JsonObject("text", true, "icons", new JsonObject("secondary", "ui-icon-carat-1-e")));
            selectUser.Click(UserChallenges.SelectUser);

            // Wire up the challenge event for a specific user
            jQueryUIObject requestMatch = (jQueryUIObject)this.Obj.Find(".requestMatch");
            requestMatch.Button(new JsonObject("text", true, "icons", new JsonObject("secondary", "ui-icon-carat-1-e")));
            requestMatch.Click(Players.RequestMatch);

            jQueryUIObject selects = (jQueryUIObject)this.Obj.Find("th select");
            selects.Each((ElementIterationCallback)delegate(int index, Element el)
            {
                ((jQueryUIObject)jQuery.FromElement(el)).MultiSelect(new JsonObject(
                    "header", false, 
                    "minWidth", "80", 
                    "height", "auto", 
                    "noneSelectedText", el.Title, 
                    "selectedText", el.Title, 
                    "close", (Callback)delegate() { DoFilter(this.Obj, true); }));
            });

            this.DoFilter(this.Obj, false);
        }

        public void DoFilter(jQueryObject obj, bool postBack)
        {
            jQueryUIObject selects = (jQueryUIObject)obj.Find("th select");
            string filterValue = "";

            selects.Each((ElementIterationCallback)delegate(int index, Element el)
            {
                jQueryUIObject select = (jQueryUIObject)jQuery.FromElement(el);
                Array checkedItems = select.MultiSelect("getChecked");

                if (checkedItems.Length > 0)
                {
                    if (filterValue.Length > 0)
                    {
                        filterValue = filterValue + ",,";
                    }

                    filterValue = filterValue + select.GetAttribute("name") + "=";

                    for (int i = 0; i < checkedItems.Length; ++i)
                    {
                        if (i > 0)
                        {
                            filterValue = filterValue + "||";
                        }

                        filterValue = filterValue + ((InputElement)checkedItems[i]).Value;
                    }

                }
            });

            if (this.Filter != filterValue)
            {
                this.Filter = filterValue;

                if (postBack)
                {
                    PostBack(0);
                }
            }
        }
    }
}