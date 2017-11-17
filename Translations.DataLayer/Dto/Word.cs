using System;
using System.Collections.Generic;

namespace Translations.DataLayer.Dto
{
    public class Word
    {
        public Guid Id { get; set;  }
        public List<TranslatedWord> Translations { get; set; }
        public string Description { get; set; }
    }
}
