using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Connected.Common
{
    public enum IoCLifeTimeType
    {
        Singleton,
        PerRequest,
        PerThread,
        ContainerControllled
    }

    public class NotRegisteredException : Exception
    {
        public NotRegisteredException () : base() { }

        public NotRegisteredException (string message)
            : base(message) { }

        public NotRegisteredException (string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NotRegisteredException (string message, Exception innerException)
            : base(message, innerException) { }

        public NotRegisteredException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }

    public class IoCManager
    {
        //TODO : This one need to be private to make 'IoC Container Agnostic'
        public static readonly UnityContainer Container = null;

        private static readonly Lazy<IoCManager> _instance = new Lazy<IoCManager>(() => new IoCManager());

        public static IoCManager Instance { get { return _instance.Value; } }

        static IoCManager()
        {
            //IoC Container
            Container = new UnityContainer();
            ////_container.LoadConfiguration("unity");
            Container.AddNewExtension<Interception>();
        }

        //REGISTER
        //Register interface to class
        public void Register<T, TT>(IoCLifeTimeType ltt) where TT : class, T
        {
            var ltm = getLifeTimeManager(ltt);
            if (!Container.IsRegistered<T>())
            {
                Container.RegisterType<T, TT>(ltm).Configure<Interception>().SetInterceptorFor<T>(new InterfaceInterceptor());

            }
            //else  throw new Exception("Already registered.");
        }

        public void Register<T, TT>() where TT : class, T
        {
            if (!Container.IsRegistered<T>())
            {
                Container.RegisterType<T, TT>().Configure<Interception>().SetInterceptorFor<T>(new InterfaceInterceptor());
            }
            //else  throw new Exception("Already registered.");
        }

        public void Register<T, TT>(params InjectionMember[] injectionMembers) where TT : class, T
        {
            if (!Container.IsRegistered<T>())
            {
                Container.RegisterType<T, TT>(injectionMembers)
                    .Configure<Interception>()
                    .SetInterceptorFor<T>(new InterfaceInterceptor());
            }
            //else  throw new Exception("Already registered.");
        }

        //Register class 
        public void Register<T>(IoCLifeTimeType ltt = IoCLifeTimeType.PerRequest) where T : class
        {
            var ltm = getLifeTimeManager(ltt);
            if (!Container.IsRegistered<T>())
            {
                //TODO : Interception fails 
                Container.RegisterType<T, T>(ltm);
                //.Configure<Interception>()
                //.SetInterceptorFor<T>(new TransparentProxyInterceptor());
            }
            //else throw new Exception("bu tip zaten kayıtlı");
        }

        //Register interface to class (named)
        public void Register<T, TT>(string name, IoCLifeTimeType ltt = IoCLifeTimeType.PerRequest) where TT : class, T
        {
            var ltm = getLifeTimeManager(ltt);
            if (!Container.IsRegistered<T>(name))
            {
                Container.RegisterType<T, TT>(name, ltm)
                    .Configure<Interception>()
                    .SetInterceptorFor<T>(name, new InterfaceInterceptor());
            }
            //else throw new Exception("Already registered.");
        }

        public void RegisterInstance(object instance)
        {
            Container.RegisterInstance(instance);
        }

        ////RESOLVE
        ////Resolve interface (named)
        //public T Resolve<T>(string name)
        //{
        //    return Container.Resolve<T>(name);

        //    if (Container.IsRegistered<T>(name))
        //        return Container.Resolve<T>(name);

        //    throw new NullReferenceException("Couldn't resolve type : " + typeof(T));
        //}
        ////Resolve interface 
        //public T Resolve<T>()
        //{
        //    return Container.Resolve<T>();
        //    if (Container.IsRegistered<T>())
        //        return Container.Resolve<T>();

        //    throw new NullReferenceException("Couldn't resolve type : " + typeof(T));
        //}
        ////Resolve type
        //public object Resolve(Type type)
        //{
        //    return Container.Resolve(type);

        //    if (Container.IsRegistered(type))
        //        return Container.Resolve(type);

        //    throw new NullReferenceException("Couldn't resolve type : " + type);
        //}

        ////Resolve type
        //public T ResolveDependencies<T>()
        //{
        //    return Container.Resolve<T>();
        //}

        public T ResolveIfRegistered<T>()
        {
            if (Container.IsRegistered<T>())
                return Container.Resolve<T>();
            
            throw new NotRegisteredException("Type : " + typeof (T).Name + " isn't registered.");
        }
        public T ResolveIfRegistered<T>(string name)
        {
            if (Container.IsRegistered<T>(name))
                return Container.Resolve<T>(name);

            throw new NotRegisteredException("Type : " + typeof(T).Name + " isn't registered.");
        }

        //Lifetime manager wrapper for Unity
        private LifetimeManager getLifeTimeManager(IoCLifeTimeType ltt)
        {
            switch (ltt)
            {
                case IoCLifeTimeType.Singleton:
                    return new HierarchicalLifetimeManager();
                case IoCLifeTimeType.PerRequest:
                    return new TransientLifetimeManager();
                case IoCLifeTimeType.PerThread:
                    return new PerThreadLifetimeManager();
                case IoCLifeTimeType.ContainerControllled:
                    return new ContainerControlledLifetimeManager();
                default:
                    throw new Exception("lifetime tipi bulunamadı");
            }
        }
    }
}
