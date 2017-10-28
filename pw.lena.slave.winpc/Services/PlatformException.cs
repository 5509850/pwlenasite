using PlatformAbstractions.Interfaces;
using System;


namespace pw.lena.slave.winpc.Services
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
