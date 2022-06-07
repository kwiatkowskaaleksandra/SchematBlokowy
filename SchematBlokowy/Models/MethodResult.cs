using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;

namespace SchematBlokowy.Application
{
    public class MethodResult
    {
        public MethodResult()
        {
            Errors = new List<ErrorResult>();
        }
        public List<ErrorResult> Errors { get; set; }
        public bool IsSuccess { get { return !Errors.Any(); } }
        public void AddError(string key, string message)
        {
            Errors.Add(new ErrorResult(key, message));
        }

        public void AddErrorForMultipleKeys(string message, params string[] keys)
        {
            foreach (var key in keys)
                Errors.Add(new ErrorResult(key, message));
        }

        public void AddErrorsToModelState(ModelStateDictionary modelState)
        {
            foreach (var error in Errors)
            {
                modelState.AddModelError(error.Key, error.Message);
            }
        }
    }

    public class MethodResult<T> : MethodResult where T : class
    {
        public T Result { get; set; }
    }
    public class ErrorResult
    {
        public ErrorResult()
        {

        }
        public ErrorResult(string key, string message)
        {
            Key = key;
            Message = message;
        }
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
