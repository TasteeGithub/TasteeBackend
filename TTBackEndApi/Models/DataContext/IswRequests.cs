using System;
using URF.Core.EF.Trackable;

namespace TTBackEndApi.Models.DataContext
{
    public partial class IswRequests : Entity
    {
        public string RequestId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string RequestType { get; set; }
        public string CreatedBy { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string VerifiedBy { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string Description { get; set; }
        public string TransactionId { get; set; }
        public short? HoldTime { get; set; }
        public string CollectId { get; set; }
        public string CollectType { get; set; }
        public string Reason { get; set; }
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
    }
}