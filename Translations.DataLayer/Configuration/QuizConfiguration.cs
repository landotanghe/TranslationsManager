using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer.Configuration
{
    public class QuizConfiguration : EntityTypeConfiguration<Quiz>
    {
        public QuizConfiguration()
        {
            ToTable("Quizes");
            ConfigurePrimaryKey();

            ConfigureNavigationProperties();
            ConfigurePrimitiveProperties();
        }

        private void ConfigurePrimaryKey()
        {
            HasKey(quiz => quiz.Id);
            Property(quiz => quiz.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }


        private void ConfigureNavigationProperties()
        {
            //HasMany(quiz => quiz.Questions)
            //    .WithRequired(question => question.Quiz)
            //    .HasForeignKey(question => question.QuizId);
        }

        private void ConfigurePrimitiveProperties()
        {
            Property(translation => translation.Score).IsRequired();
        }
    }
}
