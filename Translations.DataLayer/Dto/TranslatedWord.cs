namespace Translations.DataLayer.Dto
{
    /// <summary>
    /// Represents the word in a foreign language
    /// </summary>
    public class TranslatedWord
    {
        public int Id { get; set; }
        public string LanguageIso3 { get; set; }

        public Word Word { get; set; }

        public string Value { get; set; }
        public string Description { get; set; }        
    }
}
