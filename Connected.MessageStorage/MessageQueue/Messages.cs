//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connected.MessageStorage.MessageQueue
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messages
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int MessageTypeId { get; set; }
        public System.DateTime DateTime { get; set; }
    }
}
