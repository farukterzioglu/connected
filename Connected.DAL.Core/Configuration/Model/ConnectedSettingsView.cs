//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Connected.DAL.Core;

namespace Connected.DAL.Core.Configuration.Model
{
    using System;
    using System.Collections.Generic;

    public class ConnectedSettingsViewDTO : EntityBase
    {
        public string SettingsKey { get; set; }
        public string SettingsValue { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDateTimeStamp { get; set; }
        public string CreatedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDateTimeStamp { get; set; }
        public string ModifiedUserId { get; set; }
    }
}
