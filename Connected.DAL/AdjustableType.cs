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
    
    public partial class AdjustableType
    {
        public AdjustableType()
        {
            this.Adjustable = new List<Adjustable>();
        }
    
        public int Id { get; set; }
        public string TypeName { get; set; }
    
        public virtual List<Adjustable> Adjustable { get; set; }
    }
}