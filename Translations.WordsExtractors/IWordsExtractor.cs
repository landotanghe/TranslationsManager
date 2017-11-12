using System.Collections.Generic;
using System.IO;

namespace Translations.WordsExtractors
{
    public interface IWordsExtractor
    {
        IEnumerable<string> GetWords(Stream input);
    }
}
