using System.IO;

namespace Lusk.Core
{
    public abstract class AbstractResponse
    {
        public bool Continue = true;

        public MemoryStream Content;
    }
}
