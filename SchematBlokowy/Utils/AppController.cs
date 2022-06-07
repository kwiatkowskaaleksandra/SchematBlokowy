using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace SchematBlokowy.Utils
{
    [ExceptionHandler]
    [NoCache]
    public abstract class AppController : Controller
    {

        //private static string _unknownUserId;
        private static object _lockObject = new object();

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            base.OnAuthorization(filterContext);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }

        private Logger logger = LogManager.GetCurrentClassLogger();
        [NonAction]
        protected void LogTrace(string msg)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
        }
        [NonAction]
        protected void LogInfo(string msg)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
        }
        [NonAction]
        protected void LogError(string msg, Exception ex)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
        }
        [NonAction]
        protected void LogDebug(string msg)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
        }
        [NonAction]
        protected void LogWarn(string msg)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
        }
        [NonAction]
        protected string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                try
                {
                    viewResult.View.Render(viewContext, sw);
                }
                catch (Exception ex)
                {

                }
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        [NonAction]
        protected CustomJsonResult CustomJson(object data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet, string dateTimeFormat = DateTimeFormats.IsoDateTimeFormat)
        {
            return new CustomJsonResult() { Data = data, JsonRequestBehavior = behavior, DateTimeFormat = dateTimeFormat };
        }
    }
}