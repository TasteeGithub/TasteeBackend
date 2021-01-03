namespace Tastee.Shared
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

    public enum BrandStatus
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

    public enum BannerStatus
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