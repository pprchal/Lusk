using System;
using System.IO;
using System.Linq;
using System.Text;
using Lusk.Core;

namespace Lusk
{
    public class HttpResponse : AbstractResponse
    {
        public readonly int ContentLength;
        public readonly string[] Headers;
        public readonly string ResponseString;

        public HttpResponse(string response, bool processNextRequest)
        {
            Continue = processNextRequest;
            ResponseString = response;
            var sr = new StringReader(ResponseString);
            Headers = sr.AsLines().ToArray();

            base.Content = new MemoryStream();
            var rest = sr.ReadToEnd();
            var bytes = Encoding.UTF8.GetBytes(rest);
            Content.Write(bytes, 0, bytes.Length);
            Content.Position = 0;
        }

        public static HttpResponse Single(string response) =>
            new HttpResponse(response, false);

        internal byte[] GetHeadersBytes() =>
            Encoding.UTF8.GetBytes(
                string.Join(
                    separator: "\n",
                    value: Headers.Append(Environment.NewLine).ToArray()
                )
            );

        public byte[] GetContentBytes() => Content.ToArray();
    }
}
