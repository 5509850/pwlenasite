using PlatformAbstractions.Interfaces;
using System;

namespace Specific.iOS.Services
{
    public class PlatformException : IPlatformException
    {
        public PlatformException()
            : base()
        { }

        public Type URISyntaxException()
        {
            return typeof(System.Net.WebException);
        }
    }
}
