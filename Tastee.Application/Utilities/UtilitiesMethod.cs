using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tastee.Application.Utilities
{
    public static class UtilitiesMethod
    {
        public static string GetFilenameFromUrl(string url)
        {
            return String.IsNullOrEmpty(url.Trim()) || !url.Contains(".") ? string.Empty : Path.GetFileName(new Uri(url).AbsolutePath);
        }

        public static bool TryParseJson<T>(this string @this, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }
    }
}
