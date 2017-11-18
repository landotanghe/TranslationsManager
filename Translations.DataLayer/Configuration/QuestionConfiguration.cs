using System.Data.Entity.ModelConfiguration;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer.Configuration
{
    public class QuestionConfiguration : EntityTypeConfiguration<Question>
    {
        public QuestionConfiguration()
        {
            HasKey(q => new { q.QuizId, q.Number });

            HasRequired(question => question.Quiz)
                .WithMany(quiz => quiz.Questions)
                .HasForeignKey(question => question.QuizId);

            HasRequired(q => q.Value);
            HasOptional(q => q.Answer);
        }
    }
}
