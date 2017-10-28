using System;

namespace pw.lena.Core.Data.Handlers
{
    public class ExceptionHandler : Exception
    {
        public ExceptionHandler()
            : base()
        {

            this.Data.Add("message", "notFoundServer");
            throw this;
        }

        public ExceptionHandler(string message)
            : base(message)
        { }
    }
}

