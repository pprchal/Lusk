using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Lusk.Core
{
    public abstract class AbstractServer
    {
        public abstract Address Start(
            Func<AbstractRequest, AbstractResponse> processRequest,
            Address address
        );

        public abstract string Name
        {
            get;
        }

        public TcpListener Listener
        {
            get;
            set;
        }

        public abstract string Url
        {
            get;
        }

        public abstract Task<bool> ServeClient(TcpClient client);
    }
}