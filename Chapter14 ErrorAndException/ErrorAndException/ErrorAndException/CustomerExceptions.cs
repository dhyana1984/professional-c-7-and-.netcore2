using System;
namespace ErrorAndException
{
    public class CustomerExceptions
    {
        public CustomerExceptions()
        {
        }
    }

    public class CodeCallFileFormatException: Exception
    {
        public CodeCallFileFormatException(string message) : base(message) { }

        public CodeCallFileFormatException(string message, Exception innerException): base(message, innerException)
        {

        }
    }

    public class SalesSpyFoundException : Exception
    {
        public SalesSpyFoundException(string spyname) : base($"Sales spy found, with name {spyname}") { }

        public SalesSpyFoundException(string spyname, Exception innerException) : base($"Sales spy found, with name {spyname}", innerException)
        {

        }
    }

    public class UnexceptedException : Exception
    {
        public UnexceptedException(string message) : base(message) { }
        public UnexceptedException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
