using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer
{
    /// <summary>
    /// Sample interface for faking DbContext, Don't use this 
    /// </summary>
    public interface IConnectedConfEntities
    {
        /// <summary>
        /// Sample interface for faking DbContext, Don't use this 
        /// </summary>
        IDbSet<AdapterBasic> AdapterBasic { get; }
        IDbSet<AdapterMessageType> AdapterMessageType { get; }
        IDbSet<AdapterTypeDIM> AdapterTypeDIM { get; }
        IDbSet<ConnectedSettings> ConnectedSettings { get; set; }
        IDbSet<MessageSubscriptionDetails> MessageSubscriptionDetails { get; }
        IDbSet<MessageType> MessageType { get; }
        IDbSet<ReceiveAdapterDetails> ReceiveAdapterDetails { get; }
        IDbSet<AdaptersView> AdaptersView { get;}
        IDbSet<ConnectedSettingsView> ConnectedSettingsView { get; }
        int SaveChanges();
    }

    public class FakeDbSet<T> : IDbSet<T> where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<T> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
