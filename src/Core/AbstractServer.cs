namespace Lusk.Core
{
    public abstract class AbstractServer
    {
        public abstract string Name
        {
            get;
        }

        public Address Address
        {
            get;
            protected set;
        }

        public abstract string Url
        {
            get;
        }

        public abstract Address Start(Address address);
    }
}