using System.Net.Sockets;

namespace Lusk
{
    public sealed class HttpWriter
    {
        readonly Socket HttpSocket;

        public HttpWriter(Socket socket)
        {
            HttpSocket = socket;
        }

        internal bool Write(HttpResponse response)
        {
            var b = new byte[] { (byte)'\n' };
            HttpSocket.Send(response.GetHeadersBytes());
            var bytes = response.GetContentBytes();
            var bytesWritten = HttpSocket.Send(bytes);
            var nn = HttpSocket.Poll(2000, SelectMode.SelectRead);
            return bytesWritten == bytes.Length;
        }
    }
}
