using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Translations.DataLayer.Dto;
using Translations.DataLayer.Repository;

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
        
        public async Task<IEnumerable<Definition>> GetDefinitions(string word)
        {
            using (var context = new TranslationContext())
            {
                var definitions = await context.TranslatedWords
                    .Where(t => t.Value == word)
                    .Select( t => new Definition
                    {
                        Word = t.Value,
                        Description = t.Description,
                        LanguageIso3 = t.LanguageIso3
                    })
                    .ToListAsync();
                return definitions;
            }
        }        
    }
}
