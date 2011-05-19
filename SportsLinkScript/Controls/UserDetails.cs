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
    public class UserDetails : Module
    {
        jQueryUIObject EditButton;
        jQueryUIObject SaveButton;

        public UserDetails(Element element)
            : base(element)
        {
            EditButton = (jQueryUIObject)this.Obj.Find("a.edit");
            EditButton.Click(EditDetails);

            SaveButton = (jQueryUIObject)this.Obj.Find("a.save");
            SaveButton.Click(SaveDetails);

            ((jQueryUIObject)this.Obj.Find("select")).SelectMenu();
            Utility.WireLocationAutoComplete((jQueryUIObject)this.Obj.Find(".placesAutoFill"), (jQueryUIObject)this.Obj.Find(".placesAutoValue"));

        }

        private void EditDetails(jQueryEvent e)
        {
            jQueryObject edits = this.Obj.Find(".keyvaluerow .edit");

            edits.Show(EffectDuration.Fast);
            edits.Prev(".value").Hide(EffectDuration.Fast);

            this.EditButton.Hide(EffectDuration.Fast);
            this.SaveButton.Show(EffectDuration.Fast);
        }

        private void SaveDetails(jQueryEvent e)
        {
            this.EditButton.Hide(EffectDuration.Fast);
            this.Obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");

            // Find the objects with the .edit class that are descendants of objects with .keyvaluerow class
            // These are the editable key/value pairs
            jQueryObject edits = this.Obj.Find(".keyvaluerow .edit");

            string ntrp = edits.Find(".ntrp").GetValue();
            string court = edits.Find(".placesAutoValue").GetValue();
            string playPreference = edits.Find(".preference").GetValue();
            string style = edits.Find(".style").GetValue();
            string email = ((CheckBoxElement)edits.Find(".email").GetElement(0)).Checked ? "true" : "false";

            JsonObject parameters = new JsonObject
            (
                "ntrp", ntrp,
                "preference", playPreference,
                "courtData", court,
                "style", style,
                "emailOffers", email
            );

            // Post the user data to the service
            jQuery.Post("/services/PostTennisUserDetails" + "?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                Utility.ProcessResponse((Dictionary)data);
            });
        }
    }
}
