using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Connected.DAL.Core
{
    public interface IRepository<T>
    {
        T Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }

    //TODO : Implement ISearch
    public abstract class GenericRepositoryBase<T> : IRepository<T> where T : EntityBase, new()
    {
        private readonly List<T> _identityMappingList;
        private readonly UnitOfWorkContext _unitOfWorkContext;

        protected GenericRepositoryBase(UnitOfWorkContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext ?? new UnitOfWorkContext();

            if (_unitOfWorkContext.IdentityMappingActive)
                _identityMappingList = new List<T>();
        }

        List<DBTrigger> triggers = null;
        List<DBTrigger> triggersForThis = null;
        private void GetTriggers()
        {
            //Get all db triggers
            triggers = Connected.Common.TriggerManager.Instance.GetTriggers<DBTrigger>();

            //Get triggers for this type 
            if (triggers != null && triggers.Any())
                triggersForThis = triggers.Where(x => x.ApplyOn == typeof(T)).ToList();
        }

        public virtual T Insert(T obj)
        {
            GetTriggers();

            //Run 'before insert triggers'
            if (triggersForThis != null && triggersForThis.Any())
                foreach (DBTrigger trigger in triggersForThis)
                {
                    try
                    {
                        trigger.BeforeInsert();
                    }
                    catch (Exception)
                    {
                        //TODO : Log exception
                    }
                }

            //Call to actual insert
            var newObj = OnInsert(obj);

            //Trigger after insert
            if (triggersForThis != null && triggersForThis.Any())
                foreach (DBTrigger trigger in triggersForThis)
                    try
                    {
                        trigger.AfterInsert();
                    }
                    catch (Exception)
                    {
                        //TODO : Log exception
                    }

            return newObj;
        }
        public abstract T OnInsert(T obj);

        public virtual void Update(T obj)
        {
            //@DBTrigger usage
            #region DBTrigger sample
            GetTriggers();
            
            if (triggersForThis != null && triggersForThis.Any())
            foreach (DBTrigger trigger in triggersForThis)
                trigger.BeforeUpdate();
            #endregion

            OnUpdate(obj);
        }
        public abstract void OnUpdate(T obj);

        public virtual void Delete(T obj)
        {
            OnDelete(obj);
        }
        public abstract void OnDelete(T obj);

        public virtual T GetById(int id)
        {
            string idPropName = "Id";
            var idProp = this.GetType().GetProperties().FirstOrDefault( x=> x.GetCustomAttributes(typeof(KeyAttribute),false).Any());
            if (idProp != null) idPropName = idProp.Name;

            //Query '_identityMappingList' if 'IdentityMappingActive'
            if (_unitOfWorkContext.IdentityMappingActive)
            {
                try
                {
                    var param = Expression.Parameter(typeof(T));
                    var lambda1 = Expression.Lambda<Func<T, bool>>(
                        Expression.Equal(
                            Expression.Property(param, "Id"), //That's the harcoded parameter!

                            Expression.Constant(id)),
                        param);

                    var res1 = _identityMappingList.SingleOrDefault<T>(lambda1.Compile());

                    if (res1 != null)
                        return res1;
                    //BinaryExpression equalExpr = Expression.Equal(
                    //    Expression.Property(Expression.Parameter(typeof(T)), idPropName),
                    //    Expression.Constant(id));

                    //var lambda = Expression.Lambda<Func<T, bool>>(equalExpr);

                    //var res = _identityMappingList.SingleOrDefault<T>(lambda.Compile());

                    //if (res != null)
                    //    return res;
                }
                catch (Exception ex)
                {
                    //TODO : remove exception throwing
                    throw ex;
                    // ignored
                }
            }

            T recordFromDB = OnGetById(id);
            if (recordFromDB == null) return null;

            if (!_unitOfWorkContext.IdentityMappingActive) return recordFromDB;

            //Add to identity mapping list if it is active and didn't reached to limit
            if (_unitOfWorkContext.IdentityMappingRecordLimit != null &&_identityMappingList.Count() < _unitOfWorkContext.IdentityMappingRecordLimit)
                _identityMappingList.Add(recordFromDB);

            return recordFromDB;
        }
        public abstract T OnGetById(int id);

        public virtual IEnumerable<T> GetAll()
        {
            return OnGetAll();
        }
        public abstract IEnumerable<T> OnGetAll();
    }
}
