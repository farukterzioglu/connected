//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SendAdapter.GetData.EkinCounter
{
    using System;
    using System.Collections.Generic;
    
    public partial class CounterHistorical
    {
        public int Id { get; set; }
        public int CounterId { get; set; }
        public int Value { get; set; }
        public System.DateTime DateTime { get; set; }
    
        public virtual Counters Counters { get; set; }
    }
}