using SchematBlokowy.Dictionaries;

namespace SchematBlokowy.Application
{
    public class AppUserData
    {
        public AppUserData()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public LanguageDictionary Language { get; set; }
        public string Login { get; set; }
        public bool IsActive { get; set; }
    }
}
