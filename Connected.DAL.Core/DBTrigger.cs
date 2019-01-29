using System;
using Connected.Common;

namespace Connected.DAL.Core
{
    public interface IDbTrigger
    {
        Type ApplyOn { get; }
        void BeforeInsert();
        void AfterInsert();

        void BeforeUpdate();
        void AfterUpdate();
    }

    public abstract class DBTrigger : ITrigger, IDbTrigger
    {
        public Type TriggerType
        {
            get { return typeof(DBTrigger); }
        }

        public abstract Type ApplyOn { get; }

        public virtual void BeforeInsert()
        {
            Console.WriteLine("DBTrigger.BeforeInsert");
        }

        public virtual void AfterInsert()
        {
            Console.WriteLine("DBTrigger.AfterInsert");
        }
        public virtual void BeforeUpdate()
        {
            Console.WriteLine("DBTrigger.BeforeUpdate");
        }

        public void AfterUpdate()
        {
            Console.WriteLine("DBTrigger.AfterUpdate");
        }

        public void Dispose()
        {
        }
    }

}
