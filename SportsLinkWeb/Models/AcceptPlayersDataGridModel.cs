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
    public class AcceptPlayersDataGridModel : DataGridModel
    {
        public AcceptPlayersDataGridModel(Guid offerGuid, TennisUserModel tennisUser, SportsLinkDB db)
            : base(null, tennisUser, db)
        {
            // No need for a header
            this.ShowHeader = true;

            // Get list of users who have accepted the offer
            IQueryable<Accept> accepts = db.Accept.Where(a => a.OfferId == offerGuid);

            IQueryable<TennisUserModel> tennisUsers = ModelUtils.GetTennisUsers(db);

            var acceptUsers = from a in accepts
                              join tu in tennisUsers
                              on a.FacebookId equals tu.FacebookId
                              select tu;


            this.Data = acceptUsers;
            this.Rows = acceptUsers;

            this.AddColumn("FacebookId", "", "PlayerGrid/UserPicture", null);
            this.AddColumn("Name", "Name");
            this.AddColumn("Rating", "Rating", (o) => IndexModel.FormatRating((double)o));
            this.AddColumn("Birthday", "Age", (o) => IndexModel.FormatAge((DateTime)o));
            this.AddColumn("City.Name", "Location");
            this.AddColumn("FacebookId", "Select Opponent", "PlayerGrid/UserSelect", null);
        }

        private IQueryable<TennisUserModel> Data { get; set; }

        public override IEnumerable<FilterOption> GetDistinctValues(ColumnDefinition col)
        {
            return null;
        }
    }
}