using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SportsLink;

namespace SportsLinkWeb.Models
{
    public class WeekPageModel : PageModel
    {
        public static WeekPageModel Create(int page)
        {
            return new WeekPageModel(page);
        }

        protected WeekPageModel(int page)
            : base(page, 1, 1)
        {
        }

        public override bool HasNext
        {
            get
            {
                return true;
            }
        }

        public override bool HasPrev
        {
            get
            {
                return this.Page > 0;
            }
        }
    }
}