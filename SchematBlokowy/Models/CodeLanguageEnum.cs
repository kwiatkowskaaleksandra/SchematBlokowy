
using BTC.Utils.Shared;
using System.ComponentModel.DataAnnotations;

namespace SchematBlokowy.Dictionaries
{
    public enum CodeLanguageEnum : int
    {
        [Display(ResourceType = typeof(EnumResource), Name = "CSharp")]
        CSharp = 0,
        [Display(ResourceType = typeof(EnumResource), Name = "CPlusPlus")]
        CPlusPlus = 1,
        [Display(ResourceType = typeof(EnumResource), Name = "C")]
        C = 2
    }
}
