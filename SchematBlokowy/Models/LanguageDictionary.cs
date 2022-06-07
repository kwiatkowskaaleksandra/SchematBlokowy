
using BTC.Utils.Shared;
using System.ComponentModel.DataAnnotations;

namespace SchematBlokowy.Dictionaries
{
    public enum LanguageDictionary : int
    {
        [Display(ResourceType = typeof(EnumResource), Name = "LanguageDictionary_Polish")]
        Polish = 0,
        [Display(ResourceType = typeof(EnumResource), Name = "LanguageDictionary_EnglishGB")]
        EnglishGB = 1,
        [Display(ResourceType = typeof(EnumResource), Name = "LanguageDictionary_GermanDE")]
        GermanDE = 2
    }
}
