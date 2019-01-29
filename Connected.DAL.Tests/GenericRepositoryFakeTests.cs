using System;
using Connected.DAL.Configuration.Repositories.Fake;
using Connected.DAL.Core;
using Connected.DAL.Core.Configuration.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.DAL.Tests
{
    [TestClass]
    public class GenericRepositoryFakeTests
    {
        [TestMethod]
        public void GetAll()
        {
            var repo = new GenericConfigurationRepositoryFake<AdapterBasicDTO>(new UnitOfWorkContext());
            repo.GetAll();
        }
    }
}
