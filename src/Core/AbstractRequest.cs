using System.IO;

namespace Lusk.Core
{
    public abstract class AbstractRequest
    {
        public Stream RawStream = new MemoryStream();
    }
}
