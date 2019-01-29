using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.DAL.DbTriggers.AdapterBasicTriggers
{
    public class BasicAdapterTrigger : DBTrigger
    {
        public override Type ApplyOn
        {
            get { return typeof(AdapterBasicDTO); }
        }
    }
}
