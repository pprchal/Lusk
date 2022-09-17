using Lusk.Core;
using System.IO;
using System.Linq;
using System.Text;

namespace Lusk
{
    public class HttpRequest : AbstractRequest
    {
        public Stream ContentStream;

        public string[] AllHeaders;

        public string Protocol => AllHeaders[0];

        public string[] Headers => AllHeaders.Skip(1).ToArray();

        public long Size = 0;

        public int ContentLength =>
            int.Parse(
                GetHeader("Content-Length")
                    .Split(new char[] { ':' })
                    .Last()
            );

        string GetHeader(string header) =>
            Headers.First(h => h.StartsWith(header));

        internal HttpRequest Close()
        {
            var sr = new StreamReader(RawStream);
            AllHeaders = sr.AsLines().ToArray();

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
