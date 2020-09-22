using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared
{
    /// <summary>
    /// Model for api to get pagged data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaggingModel<T>
    {
        public List<T> ListData { get; set; }
        public int TotalRows { get; set; }
    }
}
