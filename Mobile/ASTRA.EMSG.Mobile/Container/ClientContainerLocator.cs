using Autofac;

namespace ASTRA.EMSG.Mobile.Container
{
    public static class ClientContainerLocator
    {
        static  ClientContainerLocator()
        {
            Container = new ClientContainerSetup().BuildContainer();
        }

        public static IContainer Container { get; private set; }
    }
}
