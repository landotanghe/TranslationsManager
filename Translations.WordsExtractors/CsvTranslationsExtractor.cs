using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Translations.WordsExtractors
{
    public class CsvTranslationsExtractor
    {
        public List<TranslationItem> GetTranslations(Stream input)
        {
            using (var reader = new StreamReader(input))
            {
                var lines = ReadLines(reader);
                var languageIso3s = GetValuesInLine(lines.First());
                return lines.Skip(1)
                    .Select(line => ToTranslation(line, languageIso3s))
                    .Where(translation => translation.Translations.Any())
                    .ToList();
            }
        }

        private IEnumerable<string> ReadLines(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                yield return line;
            }
        }

        private TranslationItem ToTranslation(string line, string[] languageIso3s)
        {
            var words = GetValuesInLine(line);
            var translations = new List<TranslationPart>();
            
            for (int i = 0; i < words.Count(); i++)
            {
                if(!string.IsNullOrEmpty(languageIso3s[i]) && !string.IsNullOrEmpty(words[i]))
                {
                    var translation = new TranslationPart
                    {
                        LanguageIso3 = languageIso3s[i],
                        Word = words[i]
                    };
                    translations.Add(translation);
                }
            }

            return new TranslationItem
            {
                Translations = translations
            };
        }

        private string[] GetValuesInLine(string line)
        {
            return line.Split(';');
        }
    }
}