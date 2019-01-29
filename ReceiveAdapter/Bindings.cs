using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected;
using Connected.DAL.Repositories.Interfaces;
using Ninject;
using Ninject.Modules;
using Moq;


namespace ReceiveAdapter
{
    public class Bindings : NinjectModule
    {
        private readonly string _dalName;

        public Bindings(string dalName = "test")
        {
            this._dalName = dalName;
        }

        public override void Load()
        {
            //Ninject
            if (_dalName == "EF")
            {
                var context = Connected.DAL.Repositories.EntityFramework.DataContextFactory.GetDataContext();
                Bind<IReceiveAdapterRepository>().To<Connected.DAL.Repositories.EntityFramework.ReceiveAdapterRepository>().WithConstructorArgument(context);
            }
            else if (_dalName == "ADONET")
            {
                Bind<IReceiveAdapterRepository>().To<Connected.DAL.Repositories.ADONET.ReceiveAdapterRepository>();
            }
            else
            {
                //TODO : Moq DAL Repository
                throw new NotImplementedException();
            }
        }
    }
}
