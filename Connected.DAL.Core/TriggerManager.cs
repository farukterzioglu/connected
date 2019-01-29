using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.DAL.Core
{
    public interface ITrigger : IDisposable
    {
        Type TriggerType { get; }
    }

    public class TriggerManager
    {
        //ConfDBTriggerManager Instance 
        static TriggerManager _triggerManager;

        //'Type'-'Trigger List' Dictionary 
        private readonly Dictionary<Type, List<ITrigger>> _triggers
            = new Dictionary<Type, List<ITrigger>>();

        /// <summary>
        /// Returns singleton ConfDBTriggerManager instance
        /// </summary>
        public static TriggerManager Instance
        {
            get
            {
                //TODO : Implement a better Singulaton
                //thread lock + double check 
                if (_triggerManager == null)
                    _triggerManager = new TriggerManager();

                return _triggerManager;
            }
        }

        /// <summary>
        /// Get all trigger dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, List<ITrigger>> GetTriggers()
        {
            return _triggers;
        }

        /// <summary>
        /// Get triggers by Type
        /// </summary>
        /// <param name="type">Filters triggers by Type</param>
        /// <returns></returns>
        public List<ITrigger> GetTriggers(Type type)
        {
            return _triggers[type];
        }

        //TODO : Implement this, get Triggers with MEF by reading DLLs from a folder 
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="triggersFolderPath"></param>
        public void Initialize(string triggersFolderPath)
        {
            throw new NotImplementedException();

            CreateInstances(new List<Type>());
        }

        //TODO : Implement this, get Triggers with MEF by reading DLLs from a folder 
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="triggersFolderPath"></param>
        public void Initialize<T>(string triggersFolderPath) where T : ITrigger
        {
            throw new NotImplementedException();

            CreateInstances(new List<Type>());
        }

        public void Initialize<T>(List<Type> list) where T : ITrigger
        {
            var typeofT =typeof(T); 
            list = list.Where(x => x.BaseType == typeofT).ToList();
            Initialize(list);
        }

        public void Initialize(List<Type> list)
        {
            CreateInstances(list);

            //List all triggers and methods
            foreach (Type type in list)
            {
                string baseClassMethods = type.GetMethods().
                   Select(x => x.Name).
                   Aggregate("Base class methods : ", (current, str) => current + str + ", ");

                string overridenMethods = type.GetMethods().
                   Where(x => x.DeclaringType == type).
                   Select(x => x.Name).
                   Aggregate("Overriden methods : ", (current, str) => current + str + ", ");

                Console.WriteLine("Class initialized : " + type.Name);
                //Console.WriteLine("Applies on : " + ApplyOn.Name);
                Console.WriteLine(baseClassMethods);
                Console.WriteLine(overridenMethods);
                Console.WriteLine();   
            }
        }

        private void CreateInstances(List<Type> triggerList)
        {
            foreach (Type triggerType in triggerList)
            {
                if (triggerType.GetInterface("ITrigger") == null) continue;

                ITrigger instance = Activator.CreateInstance(triggerType) as ITrigger;
                if (instance == null) continue;

                if (!_triggers.ContainsKey(instance.TriggerType))
                {
                    _triggers.Add(instance.TriggerType, new List<ITrigger>() { instance });

                }
                else
                {
                    if (!_triggers[instance.TriggerType].Contains(instance))
                        _triggers[instance.TriggerType].Add(instance);
                }
            }
        }

        public void ClearTriggers()
        {
            var keyTriggersPairs = GetTriggers();

            foreach (KeyValuePair<Type, List<ITrigger>> keyTriggersPair in keyTriggersPairs)
            {
                foreach (ITrigger trigger in keyTriggersPair.Value)
                    trigger.Dispose();
                keyTriggersPair.Value.Clear();
            }
            keyTriggersPairs.Clear();
        }
    }
}


//namespace Connected.DAL.Core.Absolute
//{
//    public interface IDBTrigger
//    {
//        Type ApplyOn { get; }
//        void BeforeUpdate(EntityBase org, EntityBase newObj);
//        void AfterInsert(EntityBase org);
//    }
//    public abstract class DBTriggerBase : IDBTrigger
//    {
//        #region IDBTrigger implementation
//        public virtual void BeforeUpdate(EntityBase org, EntityBase newObj)
//        {
//        }
//        public virtual void AfterInsert(EntityBase org)
//        {
//        }
//        public abstract Type ApplyOn { get; }
//        #endregion
//    }

//    public class ConfDBTriggerManager
//    {
//        //ConfDBTriggerManager Instance 
//        static ConfDBTriggerManager _confDBTriggerManager;

//        //'Type'-'Trigger List' Dictionary 
//        private readonly Dictionary<Type, List<IDBTrigger>> _dbTriggers
//            = new Dictionary<Type, List<IDBTrigger>>();

//        /// <summary>
//        /// Get all trigger dictionary
//        /// </summary>
//        /// <returns></returns>
//        public Dictionary<Type, List<IDBTrigger>> GetTriggers()
//        {
//            return _dbTriggers;
//        }

//        /// <summary>
//        /// Get triggers by Type
//        /// </summary>
//        /// <param name="type">Filter triggers by Type</param>
//        /// <returns></returns>
//        public List<IDBTrigger> GetTriggers(Type type)
//        {
//            return _dbTriggers[type];
//        }

//        /// <summary>
//        /// Returns singleton ConfDBTriggerManager instance
//        /// </summary>
//        public static ConfDBTriggerManager Instance
//        {
//            get
//            {
//                //TODO : Implement a better Singulaton
//                //thread lock + double check 
//                if (_confDBTriggerManager == null)
//                    _confDBTriggerManager = new ConfDBTriggerManager();

//                return _confDBTriggerManager;
//            }
//        }

//        //TODO : Get triggers from a folder with MEF
//        //TODO : Make this private
//        public List<Type> ReadTriggerList()
//        {
//            List<Type> triggerTypes = new List<Type>()
//            {
//                typeof(AdapterBasicTrigger),
//                typeof(AdapterTypeTrigger),
//                typeof(AdapterTypeTrigger2)
//            };
//            return triggerTypes;
//        }

//        /// <summary>
//        /// Initialize Configuration DB Triggers
//        /// </summary>
//        public void Initialize()
//        {
//            var triggerTypes = ReadTriggerList();

//            foreach (Type triggerType in triggerTypes)
//            {
//                if (triggerType.GetInterface("IDBTrigger") == null) continue;

//                IDBTrigger instance = Activator.CreateInstance(triggerType) as IDBTrigger;
//                if (instance == null) continue;

//                if (!_dbTriggers.ContainsKey(instance.ApplyOn))
//                {
//                    _dbTriggers.Add(instance.ApplyOn, new List<IDBTrigger>() { instance });

//                }
//                else
//                {
//                    if (!_dbTriggers[instance.ApplyOn].Contains(instance))
//                        _dbTriggers[instance.ApplyOn].Add(instance);
//                }
//            }
//        }

//        /// <summary>
//        /// Initialize Configuration DB Triggers, 
//        /// </summary>
//        /// <param name="dbTriggersFolderPath">Path that trigger DLLs resides</param>
//        public void Initialize(string dbTriggersFolderPath)
//        {
//            //TODO : Use path parameter for searching
//            //TODO : Get triggers from a folder with MEF
//            throw new NotImplementedException();
//        }
//    }
//}

////TODO : Remove this, for testing only
//namespace Connected.DAL.Core.Absolute
//{
//    public class AdapterBasic { }
//    public class AdapterBasicTrigger : DBTriggerBase
//    {
//        public AdapterBasicTrigger()
//        {
//            Console.WriteLine("AdapterBasicTrigger initialized.");
//        }

//        public override Type ApplyOn
//        {
//            get { return typeof(AdapterBasic); }
//        }

//        /// <summary>
//        /// Restarts dispatcher service
//        /// </summary>
//        /// <param name="org"></param>
//        public override void AfterInsert(EntityBase org)
//        {
//            Console.WriteLine("AdapterBasicTrigger-AfterInsert");
//        }
//    }

//    public class AdapterTypeDIM { }
//    public class AdapterTypeTrigger : DBTriggerBase
//    {
//        public AdapterTypeTrigger()
//        {
//            Console.WriteLine("AdapterTypeTrigger initialized.");
//        }

//        public override Type ApplyOn
//        {
//            get { return typeof(AdapterTypeDIM); }
//        }
//        public override void AfterInsert(EntityBase org)
//        {
//            Console.WriteLine("AdapterBasicTrigger-AfterInsert");
//        }
//    }
//    public class AdapterTypeTrigger2 : DBTriggerBase
//    {
//        public AdapterTypeTrigger2()
//        {
//            Console.WriteLine("AdapterTypeTrigger2 initialized.");
//        }

//        public override Type ApplyOn
//        {
//            get { return typeof(AdapterTypeDIM); }
//        }

//        public override void AfterInsert(EntityBase org)
//        {
//            Console.WriteLine("AdapterBasicTrigger-AfterInsert");
//            //
//        }

//        public override void BeforeUpdate(EntityBase org, EntityBase newObj)
//        {
//            Console.WriteLine("AdapterBasicTrigger-BeforeUpdate");
//            //
//        }
//    }
//}
