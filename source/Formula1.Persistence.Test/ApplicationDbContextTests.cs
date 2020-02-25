using Formula1.Core.Entities;
using Formula1.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Formula1.PersistenceTests
{
  [TestClass()]
  public class ApplicationDbContextTests
  {
    private ApplicationDbContext GetDbContext(string dbName)
    {
      // Build the ApplicationDbContext 
      //  - with InMemory-DB
      return new ApplicationDbContext(
        new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(dbName)
          .EnableSensitiveDataLogging()
          .Options);
    }

    [TestMethod]
    public async Task D01_FirstDataAccessTest()
    {
      string dbName = Guid.NewGuid().ToString();

      using (ApplicationDbContext dbContext = GetDbContext(dbName))
      {
        Team team = new Team
        {
          Name = "Red Bull",
          Nationality = "Austria"
        };
        dbContext.Teams.Add(team);
        await dbContext.SaveChangesAsync();
      }
      using (ApplicationDbContext dbContext = GetDbContext(dbName))
      {
        var firstOrDefault = dbContext.Teams.FirstOrDefault(t => t.Name == "Red Bull");
        Assert.IsNotNull(firstOrDefault, "Zumindest ein Team muss in DB sein");
        Assert.AreEqual("Red Bull", firstOrDefault.Name);
      }
    }
  }
}