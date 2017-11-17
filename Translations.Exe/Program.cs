﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Translations.DataLayer;

namespace TranslatorStore
{
    public class Program
    {
        //private bool IsRunning = true;

        public void Run()
        {
            while (true)
            {
                var repo = new TranslationsRepository();
                var defs = repo.GetDefinitions("cow").Result;
                Console.WriteLine(defs.Count());
                repo.AddNewWordAsync("eng", "cow", "a foor footed animal that moos").Wait();
            }
        }
        //{
        //    store.AddTranslation("koe", "cow", Language.English);
        //    store.AddTranslation("schaap", "sheep", Language.English);
        //    store.AddTranslation("kip", "chicken", Language.English);
        //    store.AddTranslation("aap", "monkey", Language.English);

        //    store.Categorise("dieren", "koe", "schaap", "kip");
        //    store.Categorise("dieren", "aap");


        //    var word = store.GetWord("koe");
        //    var word2 = store.GetWord("koetje");
        //    var words = store.GetWords(new List<string> { "koe", "kip", "schaap" });
        //    var dieren = store.GetWordsInCategory("dieren");

        //    var wordsQuery = new Neo4jQueryable<Word>();


        //    //string[] test = { "koe", "schaap"};
        //    //var qsdfdf = wordsQuery
        //    //    .Where(x => test.Contains(x.Name))
        //    //    .ToList();

        //    var xx =
        //        wordsQuery.Where(w => w.Name == "koe")
        //        .FirstOrDefault();

        //    //var qsdf =
        //    //    wordsQuery.Where(w => "koe" == w.Name)
        //    //    .FirstOrDefault();

        //    //var wordsResult =
        //    //    wordsQuery.Where(w => w.Name != "koe")
        //    //    .ToList();


        //    RunLoop();
        //}

        //private void RunLoop()
        //{
        //    using (var translator = new GoogleTranslateApi())
        //    {
        //        //store.AddTranslation("koe", "cow", Language.English);
        //        //store.AddTranslation("schaap", "sheep", Language.English);
        //        //store.AddTranslation("kip", "chicken", Language.English);
        //        //store.Categorise("dieren", "koe", "schaap", "kip");

        //        //var dieren = store.GetWordsInCategory("dieren");
        //        //foreach(var dier in dieren)
        //        //{
        //        //    Console.WriteLine(dier);
        //        //}


        //        //Console.Write(store.Test());
        //        while (IsRunning)
        //        {
        //            try
        //            {
        //                InputCycle(translator);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(":( command could not be processed", ex);
        //            }
        //        }
        //    }
        //}

        //private void InputCycle(GoogleTranslateApi translator)
        //{
        //    Console.Write(":) ");
        //    var input = Console.ReadLine();
        //    var inputParts = input.Split(' ');

        //    var command = inputParts[0];
        //    var arguments = inputParts.Skip(1).ToList();

        //    if (command == "quit")
        //    {
        //        IsRunning = false;
        //    }
        //    else if(command == "translate")
        //    {
        //        var word = arguments[0];
        //        Translate(translator, word);
        //        Console.WriteLine(translator.LatestResult);
        //    }else if(command == "add")
        //    {
        //        var lang = arguments[0];
        //        var word = arguments[1];
        //        var translation = arguments[2];
        //        store.AddTranslation(word, translation, lang.GetEnum());
        //    }else if(command == "categorise")
        //    {
        //        var category = arguments[0];
        //        var words = arguments.Skip(1).ToArray();
        //        store.Categorise(category, words);
        //    }else if(command == "category")
        //    {
        //        var category = arguments[0];
        //        var words = store.GetWordsInCategory(category);
        //        foreach (var word in words)
        //        {
        //            Console.WriteLine(word);
        //        }
        //    }
        //}

        //private static void Translate(GoogleTranslateApi translator, string input)
        //{
        //    translator.Clear();
        //    translator.SetInput(input);
        //    translator.UpdateTranslation();
        //}

        static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}
