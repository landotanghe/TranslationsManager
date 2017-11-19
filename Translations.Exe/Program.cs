using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translations.DataLayer;
using Translations.DataLayer.Repository;
using Translations.WordsExtractors;

namespace TranslatorStore
{
    public class Program
    {
        private bool IsRunning = true;
        private GoogleTranslateApi translator;//= new GoogleTranslateApi();
        private ITranslationsRepository _translationsRepository = new TranslationsRepository();
        private CsvTranslationsExtractor _csvTranslationsExtractor = new CsvTranslationsExtractor();

        public void Run()
        {
            while (IsRunning)
            {
                ProcessCommand();
            }
        }

        private void ProcessCommand()
        {
            Console.Write(":) ");
            var input = Console.ReadLine();
            var inputParts = input.Split(' ');

            var command = inputParts[0];
            var arguments = inputParts.Skip(1).ToList();

            if (command == "quit")
            {
                IsRunning = false;
            }
            else if (command == "translate")
            {
                var word = arguments[0];
                Translate(translator, word);
                Console.WriteLine(translator.LatestResult);
            }
            else if (command == "list") {
                var languageIso3 = arguments[0];
                var length = int.Parse(arguments[1]);

                var translations = _translationsRepository.GetTranslations(languageIso3, length).Result;
                foreach(var t in translations)
                {
                    Console.WriteLine($"{t.Word} = {t.TranslatedWord}");
                }
            }
            else if (command == "csv")
            {
                var filePath = arguments[0];
                using (var fs = File.Open(filePath, FileMode.Open))
                {
                    var translations = _csvTranslationsExtractor.GetTranslations(fs);
                    var sentences = GetSentences(translations);
                    var words = translations.Except(sentences);

                    foreach (var word in words)
                    {
                        var dutch = GetDutchPart(word);
                        if (dutch == null)
                        {
                            continue;
                        }
                        var translatedWords = word.Translations.Where(w => w != dutch);
                        foreach (var translatedWord in translatedWords)
                        {
                            Console.WriteLine($"{translatedWord.LanguageIso3}  {dutch.Word}   {translatedWord.Word}");
                            _translationsRepository.AddNewWordAsync(translatedWord.LanguageIso3, dutch.Word, translatedWord.Word);
                        }
                    }

                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Sentences");
                    foreach (var word in sentences)
                    {
                        var dutch = GetDutchPart(word);
                        if (dutch == null)
                        {
                            continue;
                        }
                        var translatedWords = word.Translations.Where(w => w != dutch);
                        foreach (var translatedWord in translatedWords)
                        {
                            Console.WriteLine($"{translatedWord.LanguageIso3}  {dutch.Word}   {translatedWord.Word}");
                            _translationsRepository.AddNewSentence(translatedWord.LanguageIso3, dutch.Word, translatedWord.Word, "book");
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        private static TranslationPart GetDutchPart(TranslationItem translation)
        {
            return translation.Translations.Where(t => t.LanguageIso3 == "nld").FirstOrDefault();
        }

        private static IEnumerable<TranslationItem> GetSentences(IEnumerable<TranslationItem> translations)
        {
            Func<TranslationPart, bool> containtsAtLeastToSpaces = 
                sentence => sentence.Word.Where(c => c == ' ').Count() > 2;

            var sentences = translations.Where(x => x.Translations.All(containtsAtLeastToSpaces));
            return sentences;
        }

        private class Command
        {
            public Command(string name, Action action)
            {
                Name = name;
            }
            public string Name { get; }
            public Action action { get; }
            public void Execute()
            {
                action.Invoke();
            }
        }

        private static void Translate(GoogleTranslateApi translator, string input)
        {
            translator.Clear();
            translator.SetInput(input);
            translator.UpdateTranslation();
        }

        static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}
