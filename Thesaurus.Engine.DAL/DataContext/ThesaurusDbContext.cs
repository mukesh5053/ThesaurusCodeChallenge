using Microsoft.EntityFrameworkCore;
using Thesaurus.Engine.Common;

namespace Thesaurus.Engine.DAL.DataContext
{
    public class ThesaurusDbContext : DbContext
    {
        public ThesaurusDbContext()
        {

        }

        public ThesaurusDbContext(DbContextOptions<ThesaurusDbContext> options) : base(options)
        {

        }
        public DbSet<TSynonyms> TSynonyms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
