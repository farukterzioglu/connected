//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Connected.DAL.Core;

namespace Connected.DAL.Configuration.Model
{
    using System;
    using System.Collections.Generic;

    public class ReceiveAdapterDetailsDTO : EntityBase
    {
        public int AdapterId { get; set; }
        public string AdapterServiceURI { get; set; }
        public string AdapterServiceOperation { get; set; }
        public bool IsWCFService { get; set; }
        public Nullable<int> TimesToRetry { get; set; }
        public Nullable<int> RetryInterval { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual AdapterBasicDTO AdapterBasic { get; set; }
    }
}
