using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Translations.DataLayer.Dto;

namespace Translations.DataLayer
{
    public class TranslationsRepository : ITranslationsRepository
    {
        public async Task<IEnumerable<TranslatedWord>> GetWordsAsync()
        {
            using(var context = new TranslationContext())
            {
                return await context.TranslatedWords.ToListAsync();
            }
        }

        public async Task AddNewWordAsync(string iso3, string word, string description)
        {
            var wordDto = new Word
            {
                Id = Guid.NewGuid(),
                Description = description,
                Translations = new List<TranslatedWord> {
                    new TranslatedWord
                    {
                        Value = word,
                        Description = description,
                        LanguageIso3 = iso3
                    }
                }
            };

            using(var context = new TranslationContext())
            {
                context.Words.Add(wordDto);
                await context.SaveChangesAsync();
            }
        }
    }
}
