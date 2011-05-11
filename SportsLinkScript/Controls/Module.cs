// HomePage.cs
//

using System;
using System.Collections;
using System.Html;
using jQueryApi;

namespace SportsLinkScript.Controls
{
    public class Module
    {
        protected jQueryObject Obj;
        private Element Element;
        protected Element LoadingElement;
        protected bool NeedsData = false;
        protected static ArrayList Instances = new ArrayList();

        public Module(Element element)
        {
            Instances.Add(new ModuleInstance(element, this));

            this.Element = element;
            this.Obj = jQuery.FromElement(element);
            this.NeedsData = (string)element.GetAttribute("data-async") == "true";

            if (this.NeedsData)
            {
                this.LoadingElement = Document.CreateElement("div");
                this.LoadingElement.ClassName = "loading";
                this.LoadingElement.InnerHTML = "Loading...";

                jQuery.FromElement(this.LoadingElement).InsertAfter(this.Obj.Find(".data"));

                this.Obj.Find(".data").Hide(0);
                this.LoadData();
            }
        }

        protected virtual void LoadData()
        {
        }

        public virtual void Unload()
        {
            for (int i = 0; i < Instances.Count; ++i)
            {
                ModuleInstance instance = (ModuleInstance)Instances[i];

                if (instance.Element == this.Element)
                {
                    Instances.Remove(instance);
                    return;
                }
            }
        }

        public static Module GetModule(Element element)
        {
            for (int i = 0; i < Instances.Count; ++i)
            {
                ModuleInstance instance = (ModuleInstance)Instances[i];

                if (instance.Element == element)
                {
                    return instance.Instance;
                }
            }

            return null;
        }
    }
}
