using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace TTFrontEnd.Models.SqlDataContext
{
    public partial class OperatorRoles : Entity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
