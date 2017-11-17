using System.Collections.Generic;
using System.Threading.Tasks;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer
{
    public interface ITranslationsRepository
    {
        Task AddNewWordAsync(string iso3, string word, string description);
        Task<IEnumerable<TranslatedWord>> GetWordsAsync();
    }
}