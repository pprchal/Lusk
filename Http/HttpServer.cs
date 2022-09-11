using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Lusk
{
    public class HttpServer
    {

        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="waitForStartup"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static async Task Start<S>(
            bool waitForStartup,
            Func<HttpRequest, HttpResponse> fn,
            string ip = "127.0.0.1",
            int port = 8080) where S : HttpServer, new()
        {
            var started = new AutoResetEvent(false);
            var task = Task.Run(async () =>
            {
                var server = new S().Configure(
                    fn,
                    ip,
                    port,
                    started
                );

                var serveNext = true;
                do
                {
                    using (var client = await server.Listener.AcceptTcpClientAsync())
                    {
                        serveNext = await server.Serve(client);
                    }
                }while (serveNext);
                server.Listener.Stop();
            });

            if(waitForStartup)
            {
                started.WaitOne();
            }

            await task;
        }

        protected virtual HttpServer Configure(
            Func<HttpRequest, HttpResponse> processRequest,
            string ip,
            int port,
            AutoResetEvent started)
        {
            ProcessRequest = processRequest;
            Listener = new TcpListener(IPAddress.Parse(ip), port);
            Listener.Start();
            started.Set();
            return this;
        }

        protected Func<HttpRequest, HttpResponse> ProcessRequest
        {
            get;
            private set;
        }

        protected TcpListener Listener
        {
            get;
            private set;
        }

        protected virtual async Task<bool> Serve(TcpClient client)
        {
            return await Task.Run(() =>
            {
                var socket = client.Client;
                var request = CreateReader(socket).Read();
                var response = ProcessRequest(request);

                CreateWriter(socket).Write(response);
                client.Close();
                return true;
            });
        }

        protected virtual HttpReader CreateReader(Socket socket) => new HttpReader(socket);

        protected virtual HttpWriter CreateWriter(Socket socket) => new HttpWriter(socket);
    }
}
