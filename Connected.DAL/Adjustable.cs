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
    
    public partial class Adjustable
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public string IdentifierName { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual AdjustableType AdjustableType { get; set; }
    }
}
