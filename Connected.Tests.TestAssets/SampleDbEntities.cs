using System;
using System.Collections.Generic;
using System.Linq;
using Connected.Common;
using Connected.DAL.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Tests.TestAssets
{
    public class DbEntityA : EntityBase
    {
        public DbEntityA ShallowCopy()
        {
            return (DbEntityA)this.MemberwiseClone();
        }

        public string Value1 { get; set; }

        public string Rand { get; set; }
    }
    public class DbEntityB : EntityBase
    {
        public string Value1 { get; set; }
        
        public string Rand { get; set; }
    }
}
