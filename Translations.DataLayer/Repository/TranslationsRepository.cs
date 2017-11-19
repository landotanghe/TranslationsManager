using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Translations.DataLayer.DbContexts;
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

        public async Task AddNewSentence(string iso3, string sentence, string translation, string source)
        {
            var sentenceDto = new TranslatedSentence
            {
                DemonstratedWords = new List<TranslatedWord>(),
                Source = source,
                Translation = translation,
                Value = sentence,
                TranslationLanguageIso3 = iso3
            };
            
            using (var context = new TranslationContext())
            {
                context.TranslatedSentences.Add(sentenceDto);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddNewWordAsync(string iso3, string word, string translation)
        {
            var wordDto = new Word
            {
                Value = word,
                Description = null,
                Translations = new List<TranslatedWord> {
                    new TranslatedWord
                    {
                        Value = translation,
                        Description = null,
                        LanguageIso3 = iso3
                    }
                }
            };

            using (var context = new TranslationContext())
            {
                context.Words.Add(wordDto);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddNewWordAsync(string iso3, string word, string description, string translation, string translationDescription)
        {
            var wordDto = new Word
            {
                Value = word,
                Description = description,
                Translations = new List<TranslatedWord> {
                    new TranslatedWord
                    {
                        Value = translation,
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

        public async Task<IEnumerable<Translation>> GetTranslations(string iso3, int count)
        {
            using (var context = new TranslationContext())
            {
                return await context.TranslatedWords.Where(t => t.LanguageIso3 == iso3)
                    .Select(translation => new Translation
                    {
                        TranslatedWord = translation.Value,
                        TranslationLanguageIso3 = translation.LanguageIso3,
                        Word = translation.Word.Value
                    })
                    .Take(count)
                    .ToListAsync();
            }
        }
    }
}
