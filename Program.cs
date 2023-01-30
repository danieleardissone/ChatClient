using ChatClient.Services;
using LightInject;
using System.ComponentModel;
using System.Xml.Linq;

namespace ChatClient
{
    internal static class Program
    {
        public static ServiceContainer Container;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Container = new ServiceContainer();

            Container.Register<ITcpNetworkService, TcpNetworkService>();
            // Container.RegisterPropertyDependency<ITcpNetworkService>((factory, propertyInfo) => new TcpNetworkService());

            /*container.Register<IBar, Bar>();
            container.Register<IFoo>(factory => new Foo() { Bar = factory.GetInstance<IBar>() })
            var TcpNetworkService = (TcpNetworkService)Container.GetInstance<ITcpNetworkService>();*/

            ApplicationConfiguration.Initialize();
            Application.Run(new ChatForm((TcpNetworkService)Container.GetInstance(typeof(ITcpNetworkService))));
        }
    }
}