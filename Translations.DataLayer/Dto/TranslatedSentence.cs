using System.Collections.Generic;

namespace Translations.DataLayer.Dto
{
    public class TranslatedSentence
    {
        public int Id { get; set; }

        /// <summary>
        /// The sentence in the mother language (default dutch)
        /// </summary>
        public string Value { get; set; }

        public string Translation { get; set; }
        public string TranslationLanguageIso3 { get; set; }

        public string Source { get; set; }
        public List<TranslatedWord> DemonstratedWords { get; set; }
    }
}
