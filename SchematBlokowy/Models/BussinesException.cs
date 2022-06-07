using System;

namespace SchematBlokowy.Application
{
    public class BussinesException : Exception
    {
        public int Number { get; set; }
        public string ExceptionMessage { get; set; }

        public BussinesException(int number, string message) : base(string.Format("({0}) {1}", number.ToString(), message))
        {
            Number = number;
            ExceptionMessage = message;
        }
        public BussinesException(string message)
         : base(string.Format(message))
        {
            ExceptionMessage = message;
        }
    }
}
