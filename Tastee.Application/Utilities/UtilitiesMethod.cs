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
    }
}
