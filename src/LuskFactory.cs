using Lusk.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Lusk
{
    public static class LuskFactory
    {
        static Address LastAddress = Address.Default;

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="server"></param>
        /// <param name="fn"></param>
        /// <param name="waitForStart"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static LuskRuntime Run(
            AbstractServer server,
            bool waitForStart = true,
            Address address = null
        ) 
        {
            var tcpServer = server as AbstractTcpServer;

            if(address == null)
            {
                address = LastAddress;
            }

            var startedEvent = new AutoResetEvent(false);

            var runtime = new LuskRuntime(
                server: server,
                serverTask: Task.Run(async () =>
                {
                    LastAddress = server.Start(address);
                    startedEvent.Set();

                    var serveNext = true;
                    do
                    {
                        using (var client = await tcpServer.Listener.AcceptTcpClientAsync())
                        {
                            serveNext = await tcpServer.ServeClient(client);
                        }
                    } while (serveNext);
                    tcpServer.Listener.Stop();
                }),
                started: startedEvent
            );

            if(waitForStart)
            {
                startedEvent.WaitOne();
            }

            return runtime;
        }
    }
}
