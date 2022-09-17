using Lusk.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lusk
{
    public static class LuskFactory
    {
        static Address LastAddress = Address.Default;

        /// <summary>
        /// Start
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="fn"></param>
        /// <param name="waitForStart"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static LuskRuntime Run<S>(
            Func<AbstractRequest, AbstractResponse> fn,
            bool waitForStart = true,
            Address address = null
        ) where S : AbstractServer, new()
        {
            if(address == null)
            {
                address = LastAddress;
            }

            var startedEvent = new AutoResetEvent(false);
            var server = new S();

            var runtime = new LuskRuntime(
                server: server,
                serverTask: Task.Run(async () =>
                {
                    LastAddress = server.Start(
                        fn,
                        address
                    );
                    startedEvent.Set();

                    var serveNext = true;
                    do
                    {
                        using (var client = await server.Listener.AcceptTcpClientAsync())
                        {
                            serveNext = await server.ServeClient(client);
                        }
                    } while (serveNext);
                    server.Listener.Stop();
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
