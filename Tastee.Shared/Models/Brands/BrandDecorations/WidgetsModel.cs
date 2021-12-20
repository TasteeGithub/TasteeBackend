using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Brands.BrandDecorations
{
    public class WidgetsModel
    {
        [JsonProperty("info_widget")]
        public InfoWidgetModel InfoWidget { get; set; }
        [JsonProperty("brand_image_widget", NullValueHandling = NullValueHandling.Ignore)]
        public GeneralWidgetModel BrandImageWidget { get; set; }
        [JsonProperty("menu_widget", NullValueHandling = NullValueHandling.Ignore)]
        public MenuWidgetModel MenuWidget { get; set; }
        [JsonProperty("single_banner_widgets", NullValueHandling = NullValueHandling.Ignore)]
        public List<SingelBannerWidgetModel> SingleBannerWidget { get; set; }
        [JsonProperty("slider_banner_widgets", NullValueHandling = NullValueHandling.Ignore)]
        public List<SliderBannerWidgetModel> SliderBannerWidget { get; set; }
        [JsonProperty("group_item_widgets", NullValueHandling = NullValueHandling.Ignore)]
        public List<GroupItemWidgetModel> GroupItemWidget { get; set; }

    }

    public class GeneralWidgetModel
    {
        [JsonProperty("widget_style")]
        public int Style { get; set; }
        [JsonProperty("display_order")]
        public int DisplayOrder { get; set; }
    }

    public class GroupItemWidgetModel : GeneralWidgetModel
    {
        public string GroupId { get; set; }
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }
        [JsonProperty("font_color")]
        public string FontColor { get; set; }

    }

    public class MenuWidgetModel : GeneralWidgetModel
    {

        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }
        [JsonProperty("font_color")]
        public string FontColor { get; set; }
    }

    public class SingelBannerWidgetModel : GeneralWidgetModel
    {
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
    }

    public class SliderBannerWidgetModel : GeneralWidgetModel
    {
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Images { get; set; }

        public SliderBannerWidgetModel()
        {
            Images = new List<string>();
        }
    }

    public class InfoWidgetModel : GeneralWidgetModel
    {
        [JsonProperty("brand_name")]
        public string BrandName { get; set; }
        [JsonProperty("brand_address")]
        public string BrandAddress { get; set; }
        [JsonProperty("brand_image")]
        public string BrandImage { get; set; }
        [JsonProperty("brand_logo")]
        public string BrandLogo { get; set; }
        [JsonProperty("brand_type")]
        public short? BrandType { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }
        [JsonProperty("font_color")]
        public string FontColor { get; set; }
    }
}
