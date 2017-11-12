using System.Collections.Generic;
using Translator.Shared;
using System.Linq;
using Translations.Data.Nodes;

namespace Translations.Data
{
    public class TranslationsStore
    {
        private Language _baseLanguage;
        private string _connection;
        private string _login;
        private string _password;
        public TranslationsStore(Language baseLanguage)
        {
            _baseLanguage = baseLanguage;
        }

        public void AddTranslation(string word, string translation, Language language)
        {
            throw new System.Exception();
            //if (!FindWord(word).Any())
            //{
            //    CreateWord(word);
            //}
            //if(!FindTranslation(translation, language).Any())
            //{
            //    CreateTranslation(translation, language);
            //}
            
            //Link(word, translation, language);
        }

        public Word GetWord(string word)
        {
            throw new System.Exception();
        }
        
        public List<Word> GetWords(List<string> words)
        {
            throw new System.Exception();
        }

        public void Categorise(string category, params string[] words)
        {
            throw new System.Exception();
        }

        public IEnumerable<Word> GetWordsInCategory(string name)
        {
            throw new System.Exception();
        }

        private void Link(string category, string[] words)
        {
            throw new System.Exception();
        }

        private void Link(string word, string translation, Language language)
        {
            throw new System.Exception();
        }

        private IEnumerable<string> TranslationsOf(string name, Language language)
        {
            throw new System.Exception();
        }

        private void CreateTranslation(string name, Language language)
        {
        }

        private void CreateWord(string name)
        {
        }

        private void CreateCategory(string name)
        {

        }    
    }
}
