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
    public class PlayersDataGridModel : DataGridModel
    {
        public PlayersDataGridModel() { }

        public PlayersDataGridModel(TennisUserModel tennisUser, SportsLinkDB db)
            : base(tennisUser, db)
        {
            this.ShowHeader = true;
            var tennisUsers = ModelUtils.GetTennisUsers(db);

            this.Rows = tennisUsers.Where
            (p =>
                Math.Abs(p.Rating - tennisUser.Rating) <= 0.25 &&
                db.CoordinateDistanceMiles(p.City.Latitude, p.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude) < 15 &&
                p.FacebookId != tennisUser.FacebookId &&
                p.Gender == tennisUser.Gender
            ).OrderByDescending(p => db.CoordinateDistanceMiles(p.City.Latitude, p.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude));

            this.AddColumn("FacebookId", "", (o) => string.Format("<img src='http://graph.facebook.com/{0}/picture' />", ((TennisUserModel)o).FacebookId));
            this.AddColumn("Name", "Name", (o) => ((TennisUserModel)o).Name);
            this.AddColumn("Rating", "Rating", (o) => IndexModel.FormatRating(((TennisUserModel)o).Rating));
            this.AddColumn("Birthday", "Age", (o) => IndexModel.FormatAge(((TennisUserModel)o).Birthday));
        }
    }
}