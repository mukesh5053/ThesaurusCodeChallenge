using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thesaurus.Engine.BAL.Repositories.Interfaces;

namespace Thesaurus.Engine.BAL
{
    public class ThesaurusEngine : IThesaurusEngine
    {
        private readonly ILogger<ThesaurusEngine> _log;
        private readonly IThesaurus _thesaurus;

        public ThesaurusEngine(ILogger<ThesaurusEngine> log, IThesaurus thesaurus)
        {
            _log = log;
            this._thesaurus = thesaurus;
        }
        public async Task Start()
        {
            ConsoleKeyInfo cki;
            string input;
            try
            {
                _log.LogInformation("Start : Engine Started...");
                do
                {
                    Console.WriteLine("Press a : To add the given synonyms to the thesaurus.");
                    Console.WriteLine("Press b : To get synonyms from thesaurus.");
                    Console.WriteLine("Press c : To get all words from thesaurus.");
                    cki = Console.ReadKey();
                    _log.LogInformation("Option {key} selected.", cki.Key.ToString());
                    Console.WriteLine();
                    switch (cki.Key.ToString().ToLower())
                    {
                        case "a":
                            input = AskForInput("Please enter synonyms list (separated by , comma) you want to add to thesaurus.");
                            if (!string.IsNullOrEmpty(input))
                            {
                               string[] words = input.Split(",");
                               await _thesaurus.AddSynonymsAsync(new List<string>(words));
                               Console.WriteLine("Added synonyms to thesaurus");
                            }
                            else
                            {

                                Console.WriteLine("Invalid Input.");
                            }
                            break;
                        case "b":
                            input = AskForInput("Please enter word to get synonyms from thesaurus.");
                            DisplayOutput(await _thesaurus.GetSynonymsAsync(input));
                            break;
                        case "c":
                            DisplayOutput(await _thesaurus.GetWordsAsync());
                            break;
                        default:
                            Console.WriteLine("Invalid Selection.");
                            break;
                    }
                    Console.Write("Press any key to continue or escape to exit.");
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        return;
                    }

                } while (cki.Key != ConsoleKey.Escape);
                _log.LogInformation("Start : Engine Stopped...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
            }
        }

        private string AskForInput(string msg)
        {
            Console.Write(msg + "  ");
           string input = Console.ReadLine();
            _log.LogInformation("Input value : {input}", input);
            Console.WriteLine();
            return input;
        }

        private void DisplayOutput(IEnumerable<string> lst)
        {
            if(lst != null && lst.Any())
            {
                foreach (var item in lst)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("No records to display.");
            }

        }
    }
}
