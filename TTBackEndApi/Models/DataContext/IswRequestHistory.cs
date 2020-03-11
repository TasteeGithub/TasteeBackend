using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace TTBackEndApi.Models.DataContext
{
    public partial class IswRequestHistory: Entity
    {
        public long HistoryId { get; set; }
        public string RequestId { get; set; }
        public string RequestStatus { get; set; }
        public string TransType { get; set; }
        public string Reason { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Ip { get; set; }
    }
}
