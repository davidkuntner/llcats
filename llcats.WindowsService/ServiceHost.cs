using System.ServiceProcess;

namespace llcats.WindowsService
{
    internal class ServiceHost : ServiceBase
    {
        private static Thread serviceThread;
        private static bool stopping;
        private static NamedPipesServer namedPipesServer;

        public ServiceHost()
        {
            ServiceName = "llcats Service";
        }

        protected override void OnStart(string[] args)
        {
            Run(args);
        }

        protected override void OnStop()
        {
            Abort();
        }

        protected override void OnShutdown()
        {
            Abort();
        }

        public static void Run(string[] args)
        {
            serviceThread = new Thread(InitializeServiceThread)
            {
                Name = "llcats Service Thread",
                IsBackground = true
            };
            serviceThread.Start();
        }

        public static void Abort()
        {
            stopping = true;
        }

        private static void InitializeServiceThread()
        {
            namedPipesServer = new NamedPipesServer();
            namedPipesServer.InitializeAsync().GetAwaiter().GetResult();

            while (!stopping)
            {
                Task.Delay(100).GetAwaiter().GetResult();
            }
        }
    }
}