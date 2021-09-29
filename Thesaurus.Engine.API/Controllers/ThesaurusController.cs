using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thesaurus.Engine.BAL.Repositories.Interfaces;
using Thesaurus.Engine.BAL.Repositories.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Thesaurus.Engine.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThesaurusController : ControllerBase
    {

        private readonly ILogger<ThesaurusController> _logger;
        private readonly IThesaurus _thesaurusService;

        public ThesaurusController(ILogger<ThesaurusController> logger, IThesaurus thesaurusService)
        {
            this._logger = logger;
            this._thesaurusService = thesaurusService;
        }

        [HttpGet("{word}")]
        public async Task<ActionResult<IEnumerable<string>>> GetSynonymsAsync(string word)
        {
            try
            {
                _logger.LogInformation("GetSynonymsAsync() : Fetching Synonyms word for {word}", word);
                return Ok(await _thesaurusService.GetSynonymsAsync(word));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retreiving data from Database...");
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetWordsAsync()
        {
            try
            {
                _logger.LogInformation("GetWordsAsync() : Fetching all synonyms words.");

                return Ok(await _thesaurusService.GetWordsAsync());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retreiving data from Database...");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Guid>> AddSynonymsAsync(IEnumerable<string> synonyms)
        {
            try
            {
                if (synonyms == null)
                {
                    return BadRequest();
                }

                return Ok( await _thesaurusService.AddSynonymsAsync(synonyms));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving customer data...");
            }

        }
    }
}
