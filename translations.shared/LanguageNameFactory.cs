using System.Collections.Generic;
using System.Linq;

namespace Translator.Shared
{
    public static class LanguageNameFactory
    {
        private static Dictionary<Language, string> NameOf = new Dictionary<Language, string>
        {
            { Language.Dutch, "du"},
            { Language.French, "fr" },
            { Language.German, "de" },
            { Language.English, "en"}
        };

        public static string GetName(this Language language)
        {
            return NameOf[language];
        }

        public static Language GetEnum(this string name)
        {
            return NameOf.First(link => link.Value == name).Key;
        }
    }
}
