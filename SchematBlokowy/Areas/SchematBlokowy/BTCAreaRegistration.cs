using SchematBlokowy.Utils;
using System.Web.Mvc;

namespace SchematBlokowy.Areas.SchematBlokowy
{
    public class SchematBlokowyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return AreaNames.SchematBlokowy_Area;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SchematBlokowy_default",
                AreaNames.SchematBlokowy_Area + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}