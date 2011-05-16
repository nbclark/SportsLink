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
using System.Reflection;

namespace SportsLinkWeb.Models
{
    public abstract class DataGridModel : ModuleModel
    {
        public DataGridModel() { }

        public DataGridModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser)
        {
            this.Columns = new List<ColumnDefinition>();
        }

        public void AddColumn(string name, string title, Func<object, string> format)
        {
            this.Columns.Add(new ColumnDefinition(name, title, format));
        }

        public bool ShowHeader { get; protected set; }
        public ICollection<ColumnDefinition> Columns { get; protected set; }
        public IQueryable<object> Rows { get; protected set; }

        public string RenderCell(object row, ColumnDefinition cell)
        {
            Type type = row.GetType();
            PropertyInfo property = type.GetProperty(cell.Name);
            object cellValue = string.Empty;

            if (null != property)
            {
                cellValue = property.GetValue(row, null);
            }
            else
            {
                FieldInfo field = type.GetField(cell.Name);

                if (null != field)
                {
                    cellValue = field.GetValue(row);
                }
            }

            return cell.Format(row);
        }

        public class ColumnDefinition
        {
            public string Name;
            public string Title;
            public Func<object, string> Format;

            public ColumnDefinition(string name, string title, Func<object, string> format)
            {
                this.Name = name;
                this.Title = title;
                this.Format = format;
            }
        }
    }
}