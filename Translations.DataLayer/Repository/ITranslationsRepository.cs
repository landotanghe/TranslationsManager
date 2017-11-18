using System.Collections.Generic;
using System.Threading.Tasks;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer.Repository
{
    public interface ITranslationsRepository
    {
        Task AddNewWordAsync(string iso3, string word, string translation);
        Task<IEnumerable<TranslatedWord>> GetWordsAsync();
        Task<IEnumerable<Definition>> GetDefinitions(string word);
        Task AddNewSentence(string iso3, string sentence, string translation, string source);
    }
}