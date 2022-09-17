using Lusk.Core;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Lusk
{
    public class HttpServer : AbstractServer
    {
        public override string Name => "HTTP repeating server";

        Address Address;
        public override Address Start(
            Func<AbstractRequest, AbstractResponse> processRequest,
            Address address)
        {
            Address = address;
            ProcessRequest = processRequest;
            Listener = new TcpListener(IPAddress.Parse(Address.IP), Address.Port);
            // Listener.AllowNatTraversal(true);
            Listener.Start();
            Console.WriteLine($"{Name} - listening on: {Address}");
            return Address.Next();
        }

        protected Func<AbstractRequest, AbstractResponse> ProcessRequest
        {
            get;
            private set;
        }

        public override string Url => $"http://{Address.IP}:{Address.Port}";

        public override async Task<bool> ServeClient(TcpClient client) =>
            await Task.Run(() =>
            {
                var socket = client.Client;
                var request = CreateReader(socket).Read();
                var response = ProcessRequest(request);
                CreateWriter(socket).Write((HttpResponse)response);
                client.Close();
                return response.Continue;
            });

        protected virtual HttpReader CreateReader(Socket socket) =>
            new HttpReader(socket);

        protected virtual HttpWriter CreateWriter(Socket socket) =>
            new HttpWriter(socket);
    }
}
