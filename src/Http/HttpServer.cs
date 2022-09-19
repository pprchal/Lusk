using Lusk.Core;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Lusk
{
    public class HttpServer : AbstractTcpServer
    {
        public override string Name => "HTTP repeating server";

        public HttpServer(Func<HttpRequest, HttpResponse> processRequest)
        {
            ProcessRequest = processRequest;
        }


        readonly Func<HttpRequest, HttpResponse> ProcessRequest;

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
