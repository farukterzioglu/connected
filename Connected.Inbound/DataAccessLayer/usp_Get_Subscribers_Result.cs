//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connected.Inbound.DataAccessLayer
{
    using System;
    
    public partial class usp_Get_Subscribers_Result
    {
        public int AdapterId { get; set; }
        public string AdapterName { get; set; }
        public string AdapterType { get; set; }
        public string AdapterServiceURI { get; set; }
        public Nullable<int> TimesToRetry { get; set; }
        public Nullable<int> RetryInterval { get; set; }
        public int MessageTypeId { get; set; }
        public string MessageType { get; set; }
        public int AdapterTypeId { get; set; }
        public bool IsActive { get; set; }
        public string SubscriptionCriteria { get; set; }
        public string AdapterServiceOperation { get; set; }
        public bool IsWCFService { get; set; }
    }
}
