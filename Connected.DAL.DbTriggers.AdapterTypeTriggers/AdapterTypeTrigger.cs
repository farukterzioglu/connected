using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Configuration;
using Connected.DAL.Core;

namespace Connected.DAL.DbTriggers.AdapterTypeTriggers
{
    public class AdapterTypeTrigger : DBTrigger
    {
        public override Type ApplyOn
        {
            get { return typeof (AdapterTypeDIM); }
        }

        public override void BeforeUpdate()
        {
            base.BeforeUpdate();
        }
    }
}
