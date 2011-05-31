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
    public interface IPageModel
    {

        int Page { get; }
        int ItemsPerPage { get; }
        int Total { get; }
        bool HasNext { get; }
        bool HasPrev { get; }
    }

    public class PageModel<T> : IPageModel
    {
        public static PageModel<T> Create(int page, int perPage, IEnumerable<T> items)
        {
            return new PageModel<T>(page, perPage, items);
        }

        protected PageModel(int page, int perPage, IEnumerable<T> items)
        {
            this.Page = page;
            this.ItemsPerPage = perPage;

            var oneExtra = items.Skip(this.Skip).Take(this.ItemsPerPage + 1).ToList();
            this.Items = oneExtra.Take(this.ItemsPerPage).ToList();

            this.HasNext = (oneExtra.Count > this.ItemsPerPage);
            this.Total = this.Items.Count;
        }

        public List<T> Items { get; private set; }

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

        public virtual bool HasNext
        {
            get;
            private set;
        }

        public virtual bool HasPrev
        {
            get
            {
                return this.Page > 0;
            }
        }
    }
}