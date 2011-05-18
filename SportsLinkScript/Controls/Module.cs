using System;
using System.Collections;
using System.Html;
using jQueryApi;

namespace SportsLinkScript.Controls
{
    /// <summary>
    /// This class acts as a base class for loadable/unloadable HTML modules that wraps a HTML element
    /// Associated with the module are the following
    /// - the wrapper JQuery object
    /// - the System.Html.Element object from Script# that represents the DOM element wrapped by the module
    /// - currently unused loading element
    /// - static list of elements in the system
    /// </summary>
    public class Module
    {
        /// <summary>
        /// The wrapper JQuery object
        /// </summary>
        protected jQueryObject Obj;

        /// <summary>
        /// The script# Html.Element wrapper for the DOM element
        /// </summary>
        private Element Element;

        /// <summary>
        /// Unused
        /// </summary>
        protected Element LoadingElement;

        /// <summary>
        /// Unused
        /// </summary>
        protected bool NeedsData = false;

        /// <summary>
        /// Overall list of modules in the current Javascript execution context.
        /// </summary>
        protected static ArrayList Instances = new ArrayList();

        /// <summary>
        /// Construct a Module JS object to wrap the Html.Element representing the DOM element
        /// </summary>
        /// <param name="element"></param>
        public Module(Element element)
        {
            Instances.Add(new ModuleInstance(element, this));

            // store a reference to the JQuery and DOM elements
            this.Element = element;
            this.Obj = jQuery.FromElement(element);

            // This is currently unused code
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

        /// <summary>
        /// Currently unused
        /// </summary>
        protected virtual void LoadData()
        {
        }

        /// <summary>
        /// When a DOM element needs to be updated with new HTML, the existing module associated with the element is unloaded
        /// </summary>
        public virtual void Unload()
        {
            /// Currently, we search through all loaded modules and find the one matching the current module and remove it
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

        /// <summary>
        /// Gets the loaded module corresponding to the HTML element if it has been loaded
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
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
