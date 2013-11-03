using KnockoutGenerator.Core.Business;
using KnockoutGenerator.Core.Contracts;
using Microsoft.Practices.Unity;

namespace KnockoutGenerator.Core
{
    public static class Bootstraper
    {
        public static IUnityContainer Start()
        {
            var container = new UnityContainer();
            container.RegisterType<ICodeGenerator, CodeGenerator>();

            return container;
        }
    }
}
