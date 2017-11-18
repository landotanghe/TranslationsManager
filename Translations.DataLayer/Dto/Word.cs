using System;
using System.Collections.Generic;

namespace Translations.DataLayer.Dto
{
    /// <summary>
    /// Represents the word in the default language (Dutch)
    /// </summary>
    public class Word
    {
        public int Id { get; set; }

        public List<TranslatedWord> Translations { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
