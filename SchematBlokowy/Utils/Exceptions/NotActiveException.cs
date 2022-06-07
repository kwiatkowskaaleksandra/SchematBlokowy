using System;
using System.Web.Mvc;

namespace SchematBlokowy.Utils
{
    public class NotActiveException : Exception
    {
        public string ExceptionMessage { get; set; }
        public RedirectToRouteResult RedirectToRouteResult { get; set; }
        public NotActiveException(string message)
            : base(string.Format("{0}", message))
        {

        }
    }
}