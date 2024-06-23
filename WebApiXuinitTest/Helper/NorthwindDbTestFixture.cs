using Adventure.Works._2012.dbContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiXuinitTest.Helper
{
    public class NorthwindDbTestFixture
    {
        public NorthwindContext DbContext { get; private set; }

        public NorthwindDbTestFixture()
        {
            // Setup logic (e.g., database connection)
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "TestDatabase");
            DbContext = new NorthwindContext(optionsBuilder.Options);

            DbContext.Database.EnsureCreated();
            DbContext.Seed();

        }

        public void Dispose()
        {
            // Teardown logic (e.g., close database connection)
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }
}
