
using SchematBlokowy.Dictionaries;

namespace SchematBlokowy
{
    public class Code : Entity
    {
        public string Value { get; set; }
        public CodeLanguageEnum CodeLanguage { get; set; }
    }
}
