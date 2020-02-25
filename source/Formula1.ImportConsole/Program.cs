using Formula1.Core;
using Formula1.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Formula1.ImportConsole
{
  class Program
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine("Import der Rennen und Ergebnisse in die Datenbank");
      using (ApplicationDbContext dbContext = new ApplicationDbContext())
      {
        await ResetDatabaseAsync();

        Console.WriteLine("Daten werden von results.xml eingelesen");

        var results = ImportController.LoadResultsFromXmlIntoCollections().ToArray();
        if (results.Length == 0)
        {
          Console.WriteLine("!!! Es wurden keine Rennergebnisse eingelesen");
          return;
        }
        Console.WriteLine($"  Es wurden {results.Count()} Rennergebnisse eingelesen!");
        dbContext.Results.AddRange(results);
        Console.WriteLine("Ergebnisse werden in Datenbank gespeichert (persistiert)");
        await dbContext.SaveChangesAsync();
        Console.Write("Beenden mit Eingabetaste ...");
        Console.ReadLine();

      }

    }

    private static async Task ResetDatabaseAsync()
    {
      using (ApplicationDbContext ctx = new ApplicationDbContext())
      {
        Console.WriteLine("Löschen der Datenbank (falls sie bereits existiert) ...");
        await ctx.Database.EnsureDeletedAsync();
        Console.WriteLine("  > DONE");

        Console.WriteLine("Erstellen der Datenbank ...");
        await ctx.Database.EnsureCreatedAsync();
        Console.WriteLine("  > DONE");
        Console.WriteLine();
      }
    }
  }
}