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
    /// <summary>
    /// Script for the following 
    /// - allow the user to cancel an outstanding offer 
    /// - select a player who has accepted the user's offer for confirmation
    /// </summary>
    public class UserChallenges : Module
    {
        public UserChallenges(Element element)
            : base(element)
        {
            jQueryUIObject cancelMatch = (jQueryUIObject)this.Obj.Find(".cancelMatch");
            cancelMatch.Button(new JsonObject("text", false, "icons", new JsonObject("primary", "ui-icon-closethick")));
            cancelMatch.Click(CancelOffer);

            jQueryUIObject confirmOffers = (jQueryUIObject)this.Obj.Find(".confirmOffers");
            confirmOffers.Button();
            confirmOffers.Click(SelectUserDialog);
        }

        private void CancelOffer(jQueryEvent e)
        {
            this.Obj.Attribute("disabled", "disabled").AddClass("ui-state-disabled");
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);

            JsonObject parameters = new JsonObject("offerId", button.GetAttribute("data-offerId"));

            jQuery.Post("/services/CancelOffer?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }

        /// <summary>
        /// Shows the dialog to select users from the player grid
        /// </summary>
        /// <param name="e"></param>
        private void SelectUserDialog(jQueryEvent e)
        {
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);

            // Get the offer id from the button's attribute
            string offerId = button.GetAttribute("data-offerId");

            // Find the user selection dialog and communicate the offer id to the dialog so it can be posted on the dialog's select user event
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#playerGridCard");
            dialog.Children().First().Html("Loading...");
            dialog.Attribute("data-offerId", offerId);

            // Post the request to get the users who have accepted this offer
            JsonObject parameters = new JsonObject("page", 0, "offerId", offerId);
            jQuery.Post("/services/AcceptPlayerGrid?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );

            // Display the dialog in modal fashion
            dialog.Dialog(
                new JsonObject(
                    "width", jQuery.Window.GetWidth() - 40,
                    "height", jQuery.Window.GetHeight() - 20,
                    "modal", true,
                    "title", "Confirm the player you wish to play",
                    "closeOnEscape", true,
                    "position", "top"
                )
            );
        }
        
        /// <summary>
        /// Static method that gets called when a user is selected from the grid (the wiring is hard-coded in PlayerGrid itself for now)
        /// </summary>
        /// <param name="e"></param>
        public static void SelectUser(jQueryEvent e)
        {
            // Get the button that was selected - id should be the FacebookId of the selected user
            jQueryObject button = jQuery.FromElement(e.CurrentTarget);
            string selectedUserId = button.GetAttribute("data-fbId");
       
            // Get the offer id from the dialg attribute
            jQueryUIObject dialog = (jQueryUIObject)jQuery.Select("#playerGridCard");
            string offerId = dialog.GetAttribute("data-offerId");

            // Script.Alert("offerId: " + offerId + " uid: " + selectedUserId);

            // Post the confirmation - now that we have the offer id and the selected user 
            JsonObject parameters = new JsonObject("offerId", offerId, "uid", selectedUserId);

            dialog.Dialog("close");

            jQuery.Post("/services/ConfirmOfferFromPage?signed_request=" + Utility.GetSignedRequest(), Json.Stringify(parameters), (AjaxRequestCallback<object>)delegate(object data, string textStatus, jQueryXmlHttpRequest<object> request)
            {
                Utility.ProcessResponse((Dictionary)data);
            }
            );
        }
    }
}
