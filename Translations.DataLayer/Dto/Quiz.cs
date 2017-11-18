using System.Collections.Generic;

namespace Translations.DataLayer.Dto
{
    public class Quiz
    {
        public int Id { get; set; }
        public List<Question> Questions { get; set; }
        public int Score { get; set; }
    }
}
