using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Lusk
{
    public class HttpReader
    {
        readonly Socket HttpSocket;

        internal HttpReader(Socket httpSocket)
        {
            HttpSocket = httpSocket;
        }

        MemoryStream ReadNetworkStream(Socket socket)
        {
            var ms = new MemoryStream();

            // single process poll hack
            const int retry = 5;

            // little more than MTU
            var buff = new byte[2 * 1024];

            int bytesReceived;
            do
            {
                bytesReceived = socket.Receive(buff);
                ms.Write(buff, 0, bytesReceived);

                if (bytesReceived > 0)
                {
                    int i;
                    for (i = 0; i < retry; i++)
                    {
                        if (socket.Available > 0)
                        {
                            break;
                        }
                        else
                        {
                            // switch to another thread for a second to send data
                            // hack for single process
                            Task.Delay(100).Wait();
                        }
                    }

                    if (i == retry)
                    {
                        bytesReceived = 0;
                    }
                }
            } while (bytesReceived > 0);

            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        public virtual HttpRequest Read()
        {
            var request = new HttpRequest
            {
                RawStream = ReadNetworkStream(HttpSocket)
            };

            return request.Close();
        }
    }
}
