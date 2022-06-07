using SchematBlokowy.Application;
using SchematBlokowy.Utils;
using System.Text;
using System.Web.Mvc;

namespace SchematBlokowy.Areas.SchematBlokowy.Controllers
{
    public class SchematBlokowyController : AppController
    {


        public ActionResult Index()
        {
            SchematBlokowyIndexVM model = new SchematBlokowyIndexVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult Convert(string model, int type)
        {
            SchematBlokowyService SchematBlokowyService = new SchematBlokowyService();
            CodeDTO codeDTO = SchematBlokowyService.ConvertSchematBlokowy(model, type);
            return File(Encoding.UTF8.GetBytes(codeDTO.Content), "text/plain", codeDTO.FileName);
        }



    }
}