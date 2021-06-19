using System;

namespace Assets.Scripts.Networking.NetworkingManagers
{
    class ReceiveException : Exception
    {
        public ReceiveException() { }

        public ReceiveException(string message)
            : base(message)
        { }

        public ReceiveException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
