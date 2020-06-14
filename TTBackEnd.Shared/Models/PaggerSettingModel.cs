using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTBackEnd.Shared
{
    public class PaggerSettingModel
    {
        /// <summary>
        /// ID Table để phân trang
        /// </summary>
        public string TableId { get; set; }
        /// <summary>
        /// Url Link đến server side
        /// </summary>
        public string AjaxLink { get; set; }
        /// <summary>
        /// Cấu hình các column (Một mảng format theo json)
        /// </summary>
        public Column[] Columns { get; set; }
        /// <summary>
        /// Cấu hình danh sách PageSize lựa chọn
        /// </summary>
        public int[] PageSizeSetting { get; set; }

        public FilterField[] FilterFiled { get; set; }
        public string Data
        {
            get
            {
                string filter = string.Empty;
                filter += "{";
                if(FilterFiled != null && FilterFiled.Length > 0)
                {
                    foreach (var item in FilterFiled)
                    {
                        filter += "'" + item.FieldName + "': function () { return $('#" + item.InputControlId + "').val(); },";
                    }
                }
                filter += "}";
                return filter;
            }
        }
    }

    public class FilterField
    {
        /// <summary>
        /// Field truyền dữ liệu về server
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Id của input lấy giá trị truyền về Server
        /// </summary>
        public string InputControlId { get; set; }
    }

    public class Column
    {
        public string data { get; set; }
        public bool autoWidth { get; set; }

        [JsonConverter(typeof(PlainJsonStringConverter))]
        public string render { get; set; }
    }
}

public class PlainJsonStringConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return reader.Value;
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteRawValue((string)value);
    }
}
