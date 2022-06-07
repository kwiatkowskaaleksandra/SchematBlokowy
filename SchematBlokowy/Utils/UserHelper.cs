using BTC.Utils.Shared;
using SchematBlokowy.Application;
using System.Web;

namespace SchematBlokowy.Utils
{
    public class UserHelper
    {
        public static AppUserData GetUserData(bool force = false, string userName = "")
        {
            if (HttpContext.Current == null)
                return null;
            if (userName.IsNullOrEmpty())
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            if (userName.IsNullOrEmpty())
                throw new AuthorizationException(ErrorResource.NoLoginAdded);
            AppUserData userData = null;

            userData = HttpContext.Current.Session?[SessionVariableNames.AppUserData] as AppUserData;

#if DEBUG
            bool isTheSameUserName = true;
#else
            bool isTheSameUserName = false;
            if (userData != null)
                isTheSameUserName = userData.UserName == userName;
#endif
            if (userData == null || !isTheSameUserName || force)
            {



                if (userData == null)
                    throw new AuthorizationException(ErrorResource.NoLoginAdded);
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session[SessionVariableNames.AppUserData] = userData;
                }
            }

            return userData;
        }

        public static void RefreshUserData()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            if (userName.IsNullOrEmpty())
                return;
            AppUserData userData = HttpContext.Current.Session[SessionVariableNames.AppUserData] as AppUserData;
            if (userData != null && userData.UserName == userName)
            {

                if (userData == null)
                    throw new AuthorizationException(ErrorResource.NoLoginAdded);
                HttpContext.Current.Session[SessionVariableNames.AppUserData] = userData;
            }
        }


    }
}