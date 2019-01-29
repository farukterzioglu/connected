using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.Common.LogManagement;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Connected.Common.Aspect
{
    public class TraceAspect : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new TraceAspectHandler(container);
        }
    }

    public class TraceAspectHandler : ICallHandler
    {
        private readonly ILoger _loger;
        
        public TraceAspectHandler(IUnityContainer container)
        {
            try
            {
                _loger = IoCManager.Instance.ResolveIfRegistered<ILoger>();
            }
            catch (NotRegisteredException ex)
            {
                _loger = null;
            }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (_loger != null)
                _loger.WriteLog("Executing method named: " + input.MethodBase.Name);

            var result = getNext().Invoke(input, getNext);

            if (_loger != null)
                _loger.WriteLog("Method execution done - resulted in value: " + result.ReturnValue);

            return result;
        }

        public int Order { get; set; }
    }
}
