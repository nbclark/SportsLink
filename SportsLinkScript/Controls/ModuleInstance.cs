// HomePage.cs
//

using System;
using System.Collections;
using System.Html;
using jQueryApi;

namespace SportsLinkScript.Controls
{
    public class ModuleInstance
    {
        public ModuleInstance(Element element, Module instance)
        {
            this.Element = element;
            this.Instance = instance;
        }

        public Element Element;
        public Module Instance;
    }
}