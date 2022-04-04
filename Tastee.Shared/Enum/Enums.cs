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

    public enum SuggestBrandStatus
    {
        /// <summary>
        /// Chờ kích hoạt
        /// </summary>
        Pending,

        /// <summary>
        /// Đang hoạt động
        /// </summary>
        Approved,

        /// <summary>
        /// Tạm ngừng
        /// </summary>
        UnApproved
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

    public enum ProductSliderStatus
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

    public enum UploadFileType
    {
       File,
       Image
    }

    public enum ObjectType
    {
        Brand,
        Decoration
    }

    public enum BrandDecorationStatus
    {
        /// <summary>
        /// Draft
        /// </summary>
        Draft,
        /// <summary>
        /// Approved
        /// </summary>
        Approved,
        /// <summary>
        /// Deleted
        /// </summary>
        Deleted
    }

    public enum WidgetImageStatus
    {
        Default // TBD
    }

    public enum WidgetType
    {
        Info,
        SingelBanner,
        SliderBanner,
        BrandImage,
        GroupItem,
        Menu
    }

    public enum CommonStatus
    {
        /// <summary>
        /// Đang hoạt động
        /// </summary>
        Active,

        /// <summary>
        /// Tạm ngừng
        /// </summary>
        InActive
    }
}