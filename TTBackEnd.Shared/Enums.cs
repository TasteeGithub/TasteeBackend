using System;
using System.Collections.Generic;
using System.Text;

namespace TTBackEnd.Shared
{
    public enum AccountRoles
    {
        Administrator,
        User,
        Merchant,
        Operator
    }

    public enum AccountStatus
    {
        Pending,
        Actived,
        InActived,
        Locked,
        Closed
    }
}
