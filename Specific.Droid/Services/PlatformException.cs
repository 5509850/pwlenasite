using PlatformAbstractions.Interfaces;
using System;


namespace Specific.Droid
{
    public class PlatformException : IPlatformException
    {
        public PlatformException() : base() { }
        public Type URISyntaxException()
        {
            return typeof(Java.Net.URISyntaxException);
        }
    }
}
