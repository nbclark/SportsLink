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
    public class PageModel
    {
        public static PageModel Create(int page, int perPage, int total)
        {
            return new PageModel(page, perPage, total);
        }

        private PageModel(int page, int perPage, int total)
        {
            this.Page = page;
            this.ItemsPerPage = perPage;
            this.Total = total;
        }

        public int Page { get; private set; }
        public int ItemsPerPage { get; private set; }
        public int Total { get; private set; }

        public int Skip
        {
            get
            {
                return this.ItemsPerPage * this.Page;
            }
        }

        public bool HasNext
        {
            get
            {
                return (this.Page + 1) * this.ItemsPerPage < this.Total;
            }
        }

        public bool HasPrev
        {
            get
            {
                return this.Page > 0;
            }
        }
    }
}