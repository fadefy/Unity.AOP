using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.AOP.Test.Unity
{
    [TestClass]
    public class LifetimeManagerTest
    {
        private IUnityContainer _container = new UnityContainer();

        internal interface IInterface { }

        internal interface IAnother { }

        internal class Implementation : IInterface, IAnother { }

        [TestMethod]
        public void RegisterBothInterfaceAndClassTest()
        {
            _container.RegisterType<IInterface, Implementation>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAnother, Implementation>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<Implementation>(new ContainerControlledLifetimeManager());

            var objA = _container.Resolve<IInterface>();
            var objB = _container.Resolve<Implementation>();
            var objC = _container.Resolve<IAnother>();

            Assert.AreEqual(objA, objB);
            Assert.AreEqual(objA, objC);
            Assert.AreEqual(objB, objC);
        }

        internal interface IOpenInterface<T> { }

        internal class OpenImplementation<T> : IOpenInterface<T> { }

        [TestMethod]
        public void OpenGenericTypesTest()
        {
            _container.RegisterType(typeof(IOpenInterface<>), typeof(OpenImplementation<>));

            Assert.IsNotNull(_container.Resolve<IOpenInterface<int>>());
        }
    }
}
