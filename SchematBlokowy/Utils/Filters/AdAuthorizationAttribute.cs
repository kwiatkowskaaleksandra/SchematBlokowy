
using BTC.Utils.Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SchematBlokowy.Application;
using System.Linq;
using System.Web.Mvc;

namespace SchematBlokowy.Utils
{
    [System.Web.Mvc.Authorize]
    public class AdAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        public AdAuthorizationAttribute()
        {

        }
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new AuthorizationException(ErrorResource.NoLoginAdded);
            }

            AppUserData userData = UserHelper.GetUserData();
            if (!userData.IsActive)
                throw new NotActiveException(ErrorResource.AdAuthorizationNotActiveErrorText);

        }
    }

    public class AdAuthorizationHubConnectionAttribute : Microsoft.AspNet.SignalR.AuthorizeAttribute
    {


        public AdAuthorizationHubConnectionAttribute()
        {

        }

        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            if (request.User?.Identity?.IsAuthenticated == true)
            {
                return true;
            }
            return false;
        }
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            if (hubIncomingInvokerContext.MethodDescriptor.Attributes.Any(x => x is AllowServerAttribute))
            {
                return true;
            }
            return true;
        }
    }
}