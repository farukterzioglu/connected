using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Connected.Common.Aspect
{
    public class LogAspect : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new LogAspectCallHandler(container);
        }
    }

    public class LogAspectCallHandler : ICallHandler
    {
        public LogAspectCallHandler(IUnityContainer con)
        {
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Console.WriteLine("Logging before invoke");

            var result = getNext().Invoke(input, getNext);

            Console.WriteLine("Logging afer invoke");

            return result;
        }

        public int Order { get; set; }
    }
}
