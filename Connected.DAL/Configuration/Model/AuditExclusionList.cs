//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connected.DAL.Configuration.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AuditExclusionListDTO
    {
        public int AuditExclusionListId { get; set; }
        public Nullable<int> MessageTypeId { get; set; }
        public Nullable<bool> ShouldAuditMessageAtEntry { get; set; }
    
        public virtual MessageTypeDTO MessageType { get; set; }
    }
}
