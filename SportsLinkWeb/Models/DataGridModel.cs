using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SportsLink;
using System.Reflection;

namespace SportsLinkWeb.Models
{
    public abstract class DataGridModel : ModuleModel
    {
        public DataGridModel() { }

        public DataGridModel(string filters, TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            this.Columns = new List<ColumnDefinition>();
            this.FilterValues = new Dictionary<string, List<string>>();

            if (!string.IsNullOrEmpty(filters))
            {
                foreach (string filter in filters.Split(new string[] { ",," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] filterPieces = filter.Split('=');
                    string filterName = filterPieces[0];
                    string[] filterValues = filterPieces[1].Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                    this.FilterValues.Add(filterName, new List<string>(filterValues));
                }
            }
        }

        protected bool IsFilterChecked(string filterName, string value)
        {
            if (!this.IsPostBack)
            {
                return true;
            }

            return this.FilterValues.ContainsKey(filterName) && this.FilterValues[filterName].Contains(value);
        }

        public ColumnDefinition AddColumn(string name, string title)
        {
            return AddColumn(name, title, null);
        }

        public ColumnDefinition AddColumn(string name, string title, Func<object, string> format)
        {
            return AddColumn(name, title, null, format);
        }

        public ColumnDefinition AddColumn(string name, string title, string view, Func<object, string> format)
        {
            var def = new ColumnDefinition(name, title, view, format);
            this.Columns.Add(def);

            return def;
        }

        protected Dictionary<string, List<string>> FilterValues { get; private set; }

        public abstract IEnumerable<FilterOption> GetDistinctValues(ColumnDefinition col);

        public bool IsPostBack { get; set; }
        public bool ShowHeader { get; protected set; }
        public ICollection<ColumnDefinition> Columns { get; protected set; }
        public IQueryable<object> Rows { get; protected set; }

        public string GetCellValue(object row, ColumnDefinition cell)
        {
            object cellValue = GetCellValue(row, cell.Name);

            if (null != cell.Format)
            {
                return cell.Format(cellValue);
            }
            else
            {
                return Convert.ToString(cellValue);
            }
        }

        private object GetCellValue(object row, string name)
        {
            string[] nameParts = name.Split('.');

            Type type = row.GetType();
            PropertyInfo property = type.GetProperty(nameParts[0]);
            object cellValue = string.Empty;

            if (null != property)
            {
                cellValue = property.GetValue(row, null);
            }
            else
            {
                FieldInfo field = type.GetField(nameParts[0]);

                if (null != field)
                {
                    cellValue = field.GetValue(row);
                }
            }

            if (nameParts.Length > 1)
            {
                return GetCellValue(cellValue, string.Join(".", nameParts.Skip(1)));
            }

            return cellValue;
        }

        public class ColumnDefinition
        {
            public string Name;
            public string Title;
            public string View;
            public Func<object, string> Format;
            public bool CanFilter;

            public ColumnDefinition(string name, string title, string view, Func<object, string> format)
            {
                this.Name = name;
                this.Title = title;
                this.View = view;
                this.Format = format;
            }
        }

        public class FilterOption
        {
            public string Name;
            public string Value;
            public bool Selected;

            public FilterOption(string name, string value, bool selected)
            {
                this.Name = name;
                this.Value = value;
                this.Selected = selected;
            }
        }
    }
}