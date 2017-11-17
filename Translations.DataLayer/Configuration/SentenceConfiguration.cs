using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer.Configuration
{
    public class SentenceConfiguration : EntityTypeConfiguration<Sentence>
    {
        public SentenceConfiguration()
        {
            ConfigurePrimaryKey();
            ConfigurePrimitiveProperties();
        }

        private void ConfigurePrimaryKey()
        {
            HasKey(sentence => sentence.Id);
            Property(sentence => sentence.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        private void ConfigurePrimitiveProperties()
        {
            Property(sentence => sentence.Value).IsRequired();
            Property(sentence => sentence.Source).IsRequired();
        }
    }
}
