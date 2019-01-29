using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.DAL.Core
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }

        internal List<string> ChangedColumnNames = new List<string>();
        protected void OnPropChanged(string propName)
        {
            if (!ChangedColumnNames.Contains(propName))
            {
                //iceride degisiklikleri tutmak istersek
                //changedColumnNames.Add (propName);

                //dısarıda tutmak istersek
                //ChangeTrackingManager.Instance().Add(this, propName);
            }
        }

        //when we use activerecord
        public void Save()
        {
            //todo save to db

            //insert or update

            //change traking check
        }

        public EntityBase ShallowCopy()
        {
            return (EntityBase)this.MemberwiseClone();
        }
    }
}
