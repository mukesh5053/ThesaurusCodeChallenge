using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thesaurus.Engine.BAL.Repositories.Interfaces;
using Thesaurus.Engine.Common;
using Thesaurus.Engine.DAL.DataContext;

namespace Thesaurus.Engine.BAL.Repositories.Services
{
   public class ThesaurusService : IThesaurus
    {
        private readonly ILogger<IThesaurus> _log;
        private readonly ThesaurusDbContext _context;

        public ThesaurusService(ILogger<IThesaurus> log, ThesaurusDbContext context)
        {
            this._log = log;
            this._context = context;
        }

        public async Task<IEnumerable<string>> GetSynonymsAsync(string word)
        {
            IEnumerable<string> listSynonyms = null;
            try
            {
              

                //Get the GUID for matching Synonyms
                var filteredList =  _context.TSynonyms.Where(x => x.Synonyms.Equals(word)).Select(x => x.Guid).ToList();
                if (filteredList != null)
                {
                    //Filter TSynonyms with GUID that matches Synonyms
                    listSynonyms = await _context.TSynonyms.Where(x => filteredList.Contains(x.Guid) ).Select(x => x.Synonyms).ToListAsync();
                    if (listSynonyms != null ) // Remove search word from the list
                    {
                        listSynonyms = listSynonyms.Where(x => !x.Equals(word)).ToList();
                    }
                }
                return listSynonyms;
            }
            catch (Exception)
            {
                _log.LogError("GetSynonymsAsync() : ERROR fetching Synonyms for: {word} ", word);
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetWordsAsync()
        {
            try
            {
                //Get all Synonyms from the database.
                return await _context.TSynonyms.Select(x => x.Synonyms).ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogError($"GetWordsAsync() : ERROR fetching all words from database. {ex.Message.ToString()}");
                throw;
            }

        }

        public async Task<Guid> AddSynonymsAsync(IEnumerable<string> synonyms)
        {
            List<TSynonyms> addList = new List<TSynonyms>();

            if (synonyms == null && synonyms.Count() == 0)
            {
                _log.LogInformation("AddSynonymsAsync() : Synonyms list is empty. List should have atleast one synonyms.");
                throw new ArgumentNullException("Synonyms list is empty. List should have atleast one synonyms.");
            }

            Guid guid = Guid.NewGuid();
            try
            {
                _log.LogInformation("AddSynonymsAsync() : adding synonyms words to database");

                //Assign GUID and add to the list for bulk insertion.
                foreach (var item in synonyms)
                {
                    addList.Add(new TSynonyms { Guid = guid, Synonyms = item.Trim() });
                }
                //Bulk insertion
                await _context.TSynonyms.AddRangeAsync(addList);
                //update the database
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _log.LogError($"AddSynonymsAsync() : ERROR adding Synonyms to database. {ex.Message.ToString()}" );
                throw;
            }

            return guid;

        }
    }
}
