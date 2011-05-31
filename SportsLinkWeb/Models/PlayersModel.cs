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
using System.Data.Linq;
using LinqKit;
using System.Linq.Expressions;

namespace SportsLinkWeb.Models
{
    public class PlayersModel : ModuleModel
    {
        private static Func<SportsLinkDB, TennisUserModel, IQueryable<TennisUserModel>> CachedQuery = null;

        public PlayersModel() { }

        public PlayersModel(TennisUserModel tennisUserP, SportsLinkDB dbP)
            : base(tennisUserP)
        {
            if (null == CachedQuery)
            {
                var tennisUsers = ModelUtils.GetTennisUsersFunc();
                Expression<Func<SportsLinkDB, TennisUserModel, IQueryable<TennisUserModel>>> players =
                (SportsLinkDB db, TennisUserModel tennisUser) => tennisUsers.Invoke(db).Where
                    (p =>
                        Math.Abs(p.Rating - tennisUser.Rating) <= 0.25 &&
                        db.CoordinateDistanceMiles(p.City.Latitude, p.City.Longitude, tennisUser.City.Latitude, tennisUser.City.Longitude) < 15 &&
                        p.FacebookId != tennisUser.FacebookId &&
                        p.Gender == tennisUser.Gender
                    );

                CachedQuery = CompiledQuery.Compile<SportsLinkDB, TennisUserModel, IQueryable<TennisUserModel>>
                (
                    players.Expand()
                );
            }

            this.Players = CachedQuery(dbP, tennisUserP);
        }

        public IQueryable<TennisUserModel> Players { get; private set; }
    }
}