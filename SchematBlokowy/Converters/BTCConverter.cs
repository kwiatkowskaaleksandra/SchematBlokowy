
using SchematBlokowy.Dictionaries;

namespace SchematBlokowy
{
    public class SchematBlokowyConverter
    {
        public Code ToCode(string content, CodeLanguageEnum type)
        {
            Code code = new Code()
            {
                Value = content,
                CodeLanguage = type
            };
            return code;
        }
    }
}
