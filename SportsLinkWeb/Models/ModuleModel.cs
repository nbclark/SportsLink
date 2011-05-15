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
    public class ModuleModel
    {
        public ModuleModel() { }

        public ModuleModel(TennisUserModel tennisUser)
        {
            this.TennisUser = tennisUser;
        }

        public ModuleModel(TennisUserModel tennisUser, SportsLinkDB db)
            : this(tennisUser)
        {
        }

        public TennisUserModel TennisUser { get; private set; }
    }
}