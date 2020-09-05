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

    public enum BrandsStatus
    {
        /// <summary>
        /// Chờ kích hoạt
        /// </summary>
        Pending,
        /// <summary>
        /// Đang hoạt động
        /// </summary>
        Actived,
        /// <summary>
        /// Tạm ngừng
        /// </summary>
        InActived,
        /// <summary>
        /// Ngừng hoạt động/ khóa
        /// </summary>
        Locked
    }
}
