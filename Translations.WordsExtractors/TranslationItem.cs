using System.Collections.Generic;

namespace Translations.WordsExtractors
{
    public class TranslationItem
    {
        public List<TranslationPart> Translations { get; set; }
    }

    public class TranslationPart
    {
        public string Word { get; set; }
        public string LanguageIso3 { get; set; }
    }
}
