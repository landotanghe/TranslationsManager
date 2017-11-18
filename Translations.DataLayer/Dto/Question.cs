namespace Translations.DataLayer.Dto
{
    public class Question
    {
        public int QuizId { get; set; }
        public int Number { get; set; }
        public Quiz Quiz { get; set; }

        public TranslatedWord Value { get; set;  }
        public TranslatedWord Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int Attempts { get; set; }
    }
}
