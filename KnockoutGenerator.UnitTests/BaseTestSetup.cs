using KnockoutGenerator.Core;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace KnockoutGenerator.UnitTests
{
    public abstract class BaseTestSetup
    {
        protected IUnityContainer container;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            container = Bootstraper.Start();    
        }
    }
}
