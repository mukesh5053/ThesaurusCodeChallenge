using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thesaurus.Engine.BAL.Repositories.Interfaces
{
    public interface IThesaurus
    {
        /// <summary>
        /// Adds the given synonyms to the thesaurus
        /// </summary>
        /// <param name="synonyms">The synonyms to add.</param>
        Task<Guid> AddSynonymsAsync(IEnumerable<string> synonyms);

        /// <summary>
        /// Gets the synonyms for a given word.
        /// </summary>
        /// <param name="word">The word the synonyms of which to get.</param>
        /// <returns>The task objects representing the asynchronous operation</returns>
        Task<IEnumerable<string>> GetSynonymsAsync(string word);

        /// <summary>
        /// Gets all words from the thesaurus.
        /// </summary>
        /// <returns>The task objects representing the asynchronous operation</returns>
        Task<IEnumerable<string>> GetWordsAsync();
    }
}
