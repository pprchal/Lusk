using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lusk.Core
{
    public sealed class LuskRuntime : IDisposable
    {
        public LuskRuntime(AbstractServer server, Task serverTask, AutoResetEvent started)
        {
            Server = server;
            ServerTask = serverTask;
            Started = started;
        }

        public string Url => Server.Url;

        public readonly Task ServerTask;
        public readonly AutoResetEvent Started;
        private readonly AbstractServer Server;

        public void Dispose()
        {
            ServerTask.Wait();
        }
    }
}
