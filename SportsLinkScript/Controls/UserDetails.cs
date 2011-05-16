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
            jQueryObject edits = this.Obj.Find(".keyvaluerow .edit");

            edits.Hide(EffectDuration.Fast);
            edits.Prev(".value").Show(EffectDuration.Fast);

            this.EditButton.Show(EffectDuration.Fast);
            this.SaveButton.Hide(EffectDuration.Fast);
        }
    }
}
