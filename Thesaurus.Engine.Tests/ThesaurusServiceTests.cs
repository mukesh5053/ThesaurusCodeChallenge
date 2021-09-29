using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thesaurus.Engine.BAL.Repositories.Interfaces;
using Thesaurus.Engine.BAL.Repositories.Services;
using Thesaurus.Engine.Common;
using Thesaurus.Engine.DAL.DataContext;
using Xunit;
namespace Thesaurus.Engine.Tests
{
    public class ThesaurusServiceTests
    {
        private  IThesaurus thesaurusService;

        public ThesaurusServiceTests()
        {
            thesaurusService = GetInMemoryPersonRepository();
        }

        private IThesaurus GetInMemoryPersonRepository()
        {
            Mock<ILogger<IThesaurus>> _mockLog = new Mock<ILogger<IThesaurus>>();

            DbContextOptions<ThesaurusDbContext> options;
            var builder = new DbContextOptionsBuilder<ThesaurusDbContext>();
            builder.UseInMemoryDatabase("Thesaurus");
            options = builder.Options;
            ThesaurusDbContext personDataContext = new ThesaurusDbContext(options);
            personDataContext.Database.EnsureDeleted();
            personDataContext.Database.EnsureCreated();
            return new ThesaurusService(_mockLog.Object , personDataContext);
        }

        [Fact]
        private async Task AddSynonymsAsync_SingleItem_Test()
        {
            //Arrange
            string output = string.Empty;
            string input = "test";
            List<string> list = new List<string>();
            list.Add(input);
            //Act
             await thesaurusService.AddSynonymsAsync(list);
            IEnumerable<string> getWords = await thesaurusService.GetWordsAsync();
            //Assert
            foreach (var item in getWords)
            {
                output = item;
            }
            Assert.Equal(input, output);
        }


        [Fact]
        private async Task AddSynonymsAsyc_MultipleItems_Test()
        {
            //Arrange
            int count = 0;
            List<string> list = GetMultipleTestData();
            //Act
            await thesaurusService.AddSynonymsAsync(list);
            IEnumerable<string> getWords = await thesaurusService.GetWordsAsync();
            //Assert
            foreach (var item in getWords)
            {
                count++;
            }
            Assert.Equal(list.Count, count);
        }


        [Fact]
        private async Task AddSynonymsAync_NoItems_Test()
        {
            //Arrange
            int count = 0;
            List<string> list = new List<string>();
            //Act
            await thesaurusService.AddSynonymsAsync(list);
            IEnumerable<string> getWords = await thesaurusService.GetWordsAsync();
            //Assert
            foreach (var item in getWords)
            {
                count++;
            }
            Assert.Equal(list.Count, count);
        }

        [Fact]
        private async Task GetSynonymsAsync_With_Record_Test()
        {
            //Arrange
            List<string> output = new List<string>();

            List<string> list = GetMultipleTestData();
            //Act
            await thesaurusService.AddSynonymsAsync(list);
            IEnumerable<string> getWords = await thesaurusService.GetSynonymsAsync("Test1");
            //Assert
            foreach (var item in getWords)
            {
                output.Add(item);
            }
            Assert.Equal(2, output.Count);// there should be 2 match
            Assert.Equal(list[1], output[0]);  // test2
            Assert.Equal(list[2], output[1]);// test3

        }


        [Fact]
        private async Task GetSynonymsAsync_With_NoRecord_Test()
        {
            //Arrange
            int outputCount = 0;
            //Act
            IEnumerable<string> getWords = await thesaurusService.GetSynonymsAsync("Test9");
            //Assert
            foreach (var item in getWords)
            {
                outputCount++;
            }
            Assert.Equal(0, outputCount);// there should be 2 match

        }


        [Fact]
        private async Task GetWordsAsync_MultipleItems_Test()
        {
            //Arrange
            int count = 0;
            List<string> list = GetMultipleTestData();
            //Act
            await thesaurusService.AddSynonymsAsync(list);
            IEnumerable<string> getWords = await thesaurusService.GetWordsAsync();
            //Assert
            foreach (var item in getWords)
            {
                count++;
            }
            Assert.Equal(list.Count, count);
        }


        private List<string> GetMultipleTestData()
        {
            List<string> list = new List<string>();
            list.Add("Test1");
            list.Add("Test2");
            list.Add("Test3");
            return list;
        }

    }
}
