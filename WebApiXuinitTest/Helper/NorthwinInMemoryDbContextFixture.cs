using Adventure.Works._2012.dbContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiXuinitTest.Helper
{
    public class NorthwinInMemoryDbContextFixture : IDisposable
    {

        public NorthwindContext Context { get; private set; }
        public NorthwinInMemoryDbContextFixture() 
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            Context = new NorthwindContext(options);
            Context.Database.EnsureCreated();

            Context.Seed();
            //Load initial data into the database here
        }

        public void Dispose()
        {
           Context.Database.EnsureDeleted();
           Context.Dispose();
        }
    }
}
