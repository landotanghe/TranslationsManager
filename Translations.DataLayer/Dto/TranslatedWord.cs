using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translations.DataLayer.Dto
{
    public class TranslatedWord
    {
        [Key, Column(Order= 0)]
        public Guid Id { get; set; }
        [Key, Column(Order = 1)]
        public string LanguageIso3 { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
      //  public List<Sentence> Examples { get; set; }
    }
}
