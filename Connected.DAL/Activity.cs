//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connected.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Activity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ActivityTypeId { get; set; }
        public System.DateTime DateTime { get; set; }
        public string Description { get; set; }
    
        public virtual ActivityType ActivityType { get; set; }
        public virtual Item Item { get; set; }
    }
}
