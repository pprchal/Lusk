using System.Text;

namespace Lusk
{
    public class HttpRequest
    {
        public Stream RawStream = new MemoryStream();
        public Stream ContentStream;

        public string[] AllHeaders;
        public string Proto => AllHeaders[0];

        public string[] Headers => AllHeaders.Skip(1).ToArray();

        public long Size = 0;

        int _ContentLength = -1;
        public int ContentLength
        {
            get
            {
                if (_ContentLength < 0)
                {
                    _ContentLength = int.Parse(
                        GetHeader("Content-Length").Split(new char[] { ':' }).Last());
                }
                return _ContentLength;
            }
        }
        string GetHeader(string header) =>
            Headers.First(h => h.StartsWith(header));

        internal void WriteData(byte[] buff, int n)
        {
            RawStream.Write(buff, 0, n);
        }

        internal HttpRequest Close()
        {
            var sr = new StreamReader(RawStream);
            AllHeaders = HttpTools.ParseHeaders(sr).ToArray();

            var content = sr.ReadToEnd();

            ContentStream = new MemoryStream(Encoding.UTF8.GetBytes(content))
            {
                Position = 0
            };
            RawStream.Position = 0;
            return this;
        }
    }
}
