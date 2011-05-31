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
    public class WeekPageModel : PageModel<OfferModel>
    {
        public static WeekPageModel Create(int page, IEnumerable<OfferModel> offers)
        {
            return new WeekPageModel(page, offers);
        }

        protected WeekPageModel(int page, IEnumerable<OfferModel> offers)
            : base(page, Int32.MaxValue-1, offers)
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
                return true;
            }
        }
    }
}