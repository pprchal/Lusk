using System.Net.Sockets;

namespace Lusk
{
    public class HttpWriter
    {
        public HttpWriter(Socket socket)
        {
            HttpSocket = socket;
        }

        readonly Socket HttpSocket;

        internal virtual bool Write(HttpResponse response)
        {
            var headersBytes = response.GetHeadersBytes();
            var headersBytesSent = HttpSocket.Send(headersBytes);

            var contentBytes = response.GetContentBytes();
            var contentBytesSent = HttpSocket.Send(contentBytes);

            return (headersBytes.Length == headersBytesSent) &&
                (contentBytes.Length == contentBytesSent);
        }
    }
}
