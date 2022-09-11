//using Gordic.General;
//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace EOG.Core
//{
//    public class HttpCurlServer : HttpServer
//    {
//        readonly static IGLogger LOG = GLogManager.CurrentClassLogger();

//        /// <summary>
//        /// Factory
//        /// </summary>
//        /// <param name="waitForStartup"></param>
//        /// <param name="ip"></param>
//        /// <param name="port"></param>
//        public static Task Start(
//            bool waitForStartup,
//            Func<string, string> onCreateHttpResponse = null,
//            string ip = "127.0.0.1",
//            int port = 8080)
//        {
//            var started = new AutoResetEvent(false);
//            var task = Task.Run(() =>
//            {
//                new HttpServerTask(
//                    ip,
//                    port,
//                    started,
//                    onCreateHttpResponse
//                ).Serve();
//            });

//            if(waitForStartup)
//            {
//                started.WaitOne();
//            }

//            return task;
//        }

//        protected virtual async void Serve()
//        {
//            Server.Start();
//            Started?.Set();
//            for (int i = 0; i < 100; i++)
//            {
//                var client = await Server.AcceptTcpClientAsync();
//                var buffer = new byte[2 * 1024];
//                var stream = client.GetStream();
//                var length = stream.Read(buffer, 0, buffer.Length);

//                var incomingMessage = Encoding.UTF8.GetString(buffer, 0, length);
//                var httpResponse = CreateHttpResponse(incomingMessage);
//                buffer = CreateResponseBytes(httpResponse);
//                stream.Write(buffer, 0, buffer.Length);
//                // stream.Close();
//                Task.Delay(2000).Wait();
//                client.Close();
//            }
//        }

//        protected virtual byte[] CreateResponseBytes(string httpResponse) =>
//            Encoding.UTF8.GetBytes(httpResponse);

//        protected virtual string CreateHttpResponse(string incomingMessage) =>
//            OnCreateHttpResponse != null ?
//            OnCreateHttpResponse(incomingMessage)
//                : "HTTP/1.0 200 OK" + Environment.NewLine
//                + "Content-Length: " + incomingMessage.Length + Environment.NewLine
//                + "Content-Type: " + "text/plain" + Environment.NewLine
//                + Environment.NewLine
//                + "<h1>Hello, world!</h1>"
//                + Environment.NewLine + Environment.NewLine;
//    }
//}
