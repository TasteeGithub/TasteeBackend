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
        Active,
        InActive,
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
        Active,
        /// <summary>
        /// Tạm ngừng
        /// </summary>
        InActive,
        /// <summary>
        /// Ngừng hoạt động/ khóa
        /// </summary>
        Locked
    }
}
