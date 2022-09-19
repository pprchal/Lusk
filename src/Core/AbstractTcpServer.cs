using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Lusk.Core
{
    public abstract class AbstractTcpServer : AbstractServer
    {
        public override Address Start(Address address)
        {
            Address = address;
            Listener = new TcpListener(IPAddress.Parse(Address.IP), Address.Port);
            // Listener.AllowNatTraversal(true);
            Listener.Start();
            Console.WriteLine($"{Name} - listening on: {Address}");
            return Address.Next();
        }

        public TcpListener Listener
        {
            get;
            set;
        }

        public abstract Task<bool> ServeClient(TcpClient client);
    }
}