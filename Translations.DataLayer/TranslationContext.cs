using System.Data.Entity;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer
{
    internal class TranslationContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<TranslatedWord> TranslatedWords { get; set; }
    }
}
