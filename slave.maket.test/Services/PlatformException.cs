using PlatformAbstractions.Interfaces;
using System;

namespace slave.maket.test.Services
{
    public class PlatformException : IPlatformException
    {
        public PlatformException() : base() { }
        public Type URISyntaxException()
        {
            return typeof(Exception);
        }
    }
}
