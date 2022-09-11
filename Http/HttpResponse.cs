using System.Text;

namespace Lusk
{
    public class HttpResponse
    {
        public bool Continue = true;
        public readonly MemoryStream Content;
        public readonly int ContentLength;
        public readonly string[] Headers;
        public readonly string ResponseString;

        public HttpResponse(string response)
        {
            ResponseString = response;
            var sr = new StringReader(ResponseString);
            Headers = HttpTools.ParseHeaders2(sr).ToArray();


            Content = new MemoryStream();
            var rest = sr.ReadToEnd();
            var bytes = UTF8Encoding.UTF8.GetBytes(rest);
            Content.Write(bytes, 0, bytes.Length);
            Content.Position = 0;
        }

        public byte[] GetHeadersBytes()
        {
            var h = string.Join(
                "\n",
                Headers.Append(Environment.NewLine).ToArray()
            );
            return UTF8Encoding.UTF8.GetBytes(h);
        }

        public byte[] GetContentBytes()
        {
            return Content.ToArray();
        }
    }
}
