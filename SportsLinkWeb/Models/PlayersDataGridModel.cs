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

namespace SportsLinkWeb.Models
{
    public class PlayersDataGridModel : DataGridModel
    {
        public PlayersDataGridModel() { }

        public PlayersDataGridModel(string filter, TennisUserModel tennisUser, SportsLinkDB db)
            : base(filter, tennisUser, db)
        {
            this.ShowHeader = true;
            var tennisUsers = ModelUtils.GetTennisUsers(db);

            this.Data = tennisUsers
                .Where(p => p.FacebookId != tennisUser.FacebookId && db.CoordinateDistanceMiles(p.City.Latitude, p.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude) < 15)
                .OrderByDescending(p => db.CoordinateDistanceMiles(p.City.Latitude, p.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude));

            var rows = this.Data.Where
            (p =>
                Math.Abs(p.Rating - tennisUser.Rating) <= 0.25 &&
                p.Gender == tennisUser.Gender
            );

            foreach (string filterName in this.FilterValues.Keys)
            {
                List<string> values = this.FilterValues[filterName];

                switch (filterName)
                {
                    case "Rating":
                        {
                            rows = rows.Where(u => values.Contains(u.Rating.ToString()));
                        }
                        break;
                    case "Birthday":
                        {
                            rows = rows.Where(u => values.Contains(((u.Age / 5) * 5).ToString()));
                        }
                        break;
                    case "City.Name":
                        {
                            rows = rows.Where(u => values.Contains(u.City.LocationId.ToString()));
                        }
                        break;
                }
            }

            this.Rows = rows;

            this.AddColumn("FacebookId", "", "PlayerGrid/UserPicture", null);
            this.AddColumn("Name", "Name");
            this.AddColumn("Rating", "Rating", (o) => IndexModel.FormatRating((double)o)).CanFilter = true;
            this.AddColumn("Birthday", "Age", (o) => IndexModel.FormatAge((DateTime)o)).CanFilter = true;
            this.AddColumn("City.Name", "Location").CanFilter = true;
            this.AddColumn("FacebookId", "Challenge", "PlayerGrid/UserChallenge", null);
        }

        private IQueryable<TennisUserModel> Data { get; set; }

        public override IEnumerable<FilterOption> GetDistinctValues(ColumnDefinition col)
        {
            switch (col.Name)
            {
                case "Rating":
                    {
                        return this.Data.Select(u => u.Rating).Distinct().OrderBy(r => r).Select(r => new FilterOption(IndexModel.FormatRating(r), r.ToString(), this.IsFilterChecked(col.Name, r.ToString())));
                    }
                case "Birthday":
                    {
                        return this.Data.OrderByDescending(u => u.Birthday).Select(u => (u.Age / 5) * 5).Distinct().Select(a => new FilterOption(IndexModel.GetAgeGroup(a), a.ToString(), this.IsFilterChecked(col.Name, a.ToString())));
                    }
                case "City.Name":
                    {
                        return this.Data.Select(u => u.City).Distinct().OrderBy(c => c.Name).Select(c => new FilterOption(c.Name, c.LocationId.ToString(), this.IsFilterChecked(col.Name, c.LocationId.ToString())));
                    }
            }

            var param = Expression.Parameter(typeof(TennisUserModel), col.Name);
            var selectExpression = Expression.Lambda<Func<TennisUserModel, object>>(Expression.Convert(Expression.Field(param, col.Name), typeof(object)), param);

            var data = this.Rows.Cast<TennisUserModel>().Select(selectExpression).Select(o => col.Format(o)).Distinct().ToList();
            data.Sort();

            return data.Select(o => new FilterOption(o, o, !this.IsPostBack));
        }
    }
}