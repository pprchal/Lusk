namespace Lusk.Core
{
    public sealed class Address
    {
        public readonly string IP;
        public readonly int Port;

        public Address(string ip, int port)
        {
            IP = ip;
            Port = port;
        }

        public static Address Default => new Address("127.0.0.1", 8080);

        internal Address Next() => new Address(IP, Port + 1);

        public override string ToString() => $"{IP}:{Port}";
    }
}
