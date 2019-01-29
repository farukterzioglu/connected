using System;
using System.Collections.Generic;
using System.Linq;
using Connected.Common;
using Connected.DAL.DbTriggers.SampleTriggerReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Connected.Tests.TestAssets;
namespace Connected.DAL.Core.Tests
{
    [TestClass]
    public class GenericRepositoryBaseTests
    {
        private class GenericRepositoryTest<T> : GenericRepositoryBase<T> where T : EntityBase, new()
        {
            private readonly List<EntityBase> _dbContext;
            public GenericRepositoryTest(UnitOfWorkContext unitOfWorkContext) : base(unitOfWorkContext)
            {
                _dbContext = new List<EntityBase>()
                {
                    new DbEntityA() { Id = 1, Value1 = "10" }, 
                    new DbEntityA() { Id = 2, Value1 = "20" }, 
                    new DbEntityA() { Id = 3, Value1 = "10" }, 
                    new DbEntityA() { Id = 4, Value1 = "20" }, 
                    new DbEntityA() { Id = 5, Value1 = "10" }, 
                    new DbEntityA() { Id = 6, Value1 = "20" }, 
                    new DbEntityB() { Id = 1, Value1 = "10" },
                    new DbEntityB() { Id = 2, Value1 = "20" }
                };
            }
            public override T OnInsert(T obj)
            {
                _dbContext.Add(obj);
                return obj;
            }
            public override void OnUpdate(T obj)
            {
               ((DbEntityA) _dbContext.First()).Value1 ="10";
            }
            public override void OnDelete(T obj)
            {
                _dbContext.RemoveAt(0);

            }
            public override T OnGetById(int id)
            {
                var resList = _dbContext.Where(x => x.GetType() == typeof(T)).ToList(); 
                var res = (T) resList.FirstOrDefault(x => x.Id == id);
                
                if (res == null) return null;

                if (res.GetType() == typeof (DbEntityA))
                {
                    var dbEntityA = (res as DbEntityA);
                    if (dbEntityA != null) dbEntityA.Rand = Guid.NewGuid().ToString();
                }

                return (T)res.ShallowCopy();
            }

            public override IEnumerable<T> OnGetAll()
            {
                var resList = _dbContext.Where(x => x.GetType() == typeof(T)).ToList();
                return (IEnumerable<T>) resList.Select(x => x.ShallowCopy()).ToList();
            }
        }

        private GenericRepositoryTest<DbEntityA> _genericRepositoryTestA;
        private GenericRepositoryTest<DbEntityB> _genericRepositoryTestB;
        
        [TestInitialize]
        public void Init()
        {
            var unitOfWorkContext = new UnitOfWorkContext()
            {
                IdentityMappingActive = true,
                IdentityMappingRecordLimit = 2
            };

            _genericRepositoryTestA = new GenericRepositoryTest<DbEntityA>(unitOfWorkContext);
            _genericRepositoryTestB = new GenericRepositoryTest<DbEntityB>(unitOfWorkContext);
        }

        [TestMethod]
        public void TestInsert()
        {
            DbEntityA entity1 = _genericRepositoryTestA.Insert(new DbEntityA() {Id = 3, Value1 = "30"});
            DbEntityA entity2 = _genericRepositoryTestA.Insert(new DbEntityA() { Id = 4, Value1 = "40" });

            Assert.IsTrue(entity1 != null && entity2.Value1 != null);
        }

        [TestMethod]
        public void TestUpdate()
        {
            _genericRepositoryTestA.Update(new DbEntityA() { Id = 3, Value1 = "30" });
        }

        [TestMethod]
        public void TestInsertWithDbTriggers()
        {
            TriggerManager.Instance.Initialize<DBTrigger>(new TriggerReader().ReadTriggerList());

            DbEntityA entity1 = _genericRepositoryTestA.Insert(new DbEntityA() { Id = 3, Value1 = "30" });
            DbEntityA entity2 = _genericRepositoryTestA.Insert(new DbEntityA() { Id = 4, Value1 = "40" });
            DbEntityB entity3 = _genericRepositoryTestB.Insert(new DbEntityB() { Id = 4, Value1 = "60" });

            Assert.IsTrue(entity1 != null && entity2.Value1 != null && entity3.Value1 != null);
        }

        [TestMethod]
        public void TestGetWithoutIdentityMapping()
        {
            var unitOfWorkContext = new UnitOfWorkContext()
            {
                IdentityMappingActive = false
            };

            var genericRepositoryTestA1 = new GenericRepositoryTest<DbEntityA>(unitOfWorkContext);

            var ent1 = genericRepositoryTestA1.GetById(1);
            var ent2 = genericRepositoryTestA1.GetById(1);

            Assert.AreNotEqual(ent1, ent2);
        }

        [TestMethod]
        public void TestGetWithIdentityMapping()
        {
            var unitOfWorkContext = new UnitOfWorkContext()
            {
                IdentityMappingActive = true,
                IdentityMappingRecordLimit = 2
            };

            var genericRepositoryTestA1 = new GenericRepositoryTest<DbEntityA>(unitOfWorkContext);

            var ent1 = genericRepositoryTestA1.GetById(1);
            var ent2 = genericRepositoryTestA1.GetById(1);

            Assert.AreEqual(ent1, ent2);
        }

        [TestMethod]
        public void TestGetWithLimitedIdentityMapping()
        {
            var unitOfWorkContext = new UnitOfWorkContext()
            {
                IdentityMappingActive = true,
                IdentityMappingRecordLimit = 2
            };

            var genericRepositoryTestA1 = new GenericRepositoryTest<DbEntityA>(unitOfWorkContext);

            var ent1 = genericRepositoryTestA1.GetById(1);
            var ent2 = genericRepositoryTestA1.GetById(2);
            var ent3 = genericRepositoryTestA1.GetById(3);
            var ent4 = genericRepositoryTestA1.GetById(4);

            var ent5 = genericRepositoryTestA1.GetById(1);
            var ent6 = genericRepositoryTestA1.GetById(3);

            Assert.AreEqual(ent1, ent5);
            Assert.AreNotEqual(ent3, ent6);
        }
    }
}
