using System.Linq;
using Dex.Ef.Contracts;
using NUnit.Framework;
using Server.Dal.Provider;

namespace Server.Tests.Unit
{
    public class ModelStoreTests
    {
        private IModelStore ModelStore { get; set; }
        
        [SetUp]
        public void Setup()
        {
            ModelStore = new ModelStore();
        }

        [Test]
        public void CountTest()
        {
            var count = ModelStore.GetModels().Count();
            Assert.AreEqual(count, 4);
        }

    }
}