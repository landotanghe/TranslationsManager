using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Translations.WordsExtractors
{
    public class BookWordsExtractor
    {
        public IEnumerable<string> GetWords(Stream input)
        {
            using(var reader = new StreamReader(input))
            {
                return ReadLines(reader)
                    .SelectMany(line => line.Split(' ', '\t', ',', '.')
                                            .Where(word => word.Length > 1));
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
    }
}
